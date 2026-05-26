using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeRift.Core
{
    /// <summary>
    /// Shared attack card used by both player and enemy.
    /// </summary>
    public sealed class AttackCard
    {
        public int Id { get; }
        public int Damage { get; }

        public AttackCard(int id, int damage)
        {
            Id = id;
            Damage = damage;
        }
    }

    /// <summary>
    /// Enemy difficulty controls how many key numbers each enemy card has.
    /// </summary>
    public sealed class EnemyDifficultyProfile
    {
        public int Level { get; }
        public string Name { get; }
        public int KeyNumbersPerCard { get; }

        public EnemyDifficultyProfile(int level, string name, int keyNumbersPerCard)
        {
            Level = level;
            Name = name;
            KeyNumbersPerCard = keyNumbersPerCard;
        }
    }

    public enum BattleResult
    {
        Ongoing,
        PlayerDefeat,
        EnemyDefeat
    }

    /// <summary>
    /// Captures one enemy attack attempt and whether it chained into another.
    /// </summary>
    public sealed class EnemyAttackEvent
    {
        public int CardId { get; }
        public int Damage { get; }
        public int TriggerNumber { get; }
        public bool TriggersAnotherAttack { get; }

        public EnemyAttackEvent(int cardId, int damage, int triggerNumber, bool triggersAnotherAttack)
        {
            CardId = cardId;
            Damage = damage;
            TriggerNumber = triggerNumber;
            TriggersAnotherAttack = triggersAnotherAttack;
        }
    }

    /// <summary>
    /// Result of one player turn attempt.
    /// </summary>
    public sealed class PlayerTurnResult
    {
        public int SelectedCardId { get; init; }
        public bool AnswerCorrect { get; init; }
        public bool PlayerAttacked { get; init; }
        public int DamageToEnemy { get; init; }
        public bool CardLocked { get; init; }
        public int? LockedCardId { get; init; }
        public bool RetryRequired { get; init; }
        public string Message { get; init; } = string.Empty;
        public BattleResult BattleResult { get; init; }
        public QuestionSkipCommandType SkipCommand { get; init; }
        public IReadOnlyList<EnemyAttackEvent> EnemyAttacks { get; init; } = Array.Empty<EnemyAttackEvent>();
    }

    public interface IRandomProvider
    {
        int Next(int minValueInclusive, int maxValueExclusive);
    }

    public sealed class DefaultRandomProvider : IRandomProvider
    {
        public int Next(int minValueInclusive, int maxValueExclusive)
        {
            return Random.Shared.Next(minValueInclusive, maxValueExclusive);
        }
    }

    /// <summary>
    /// Deterministic random source used for reproducible test simulation.
    /// </summary>
    public sealed class QueueRandomProvider : IRandomProvider
    {
        private readonly Queue<int> _values;

        public QueueRandomProvider(IEnumerable<int> values)
        {
            _values = new Queue<int>(values);
        }

        public int Next(int minValueInclusive, int maxValueExclusive)
        {
            if (_values.Count == 0)
            {
                throw new InvalidOperationException("QueueRandomProvider ran out of values.");
            }

            int value = _values.Dequeue();
            if (value < minValueInclusive || value >= maxValueExclusive)
            {
                throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is outside [{minValueInclusive}, {maxValueExclusive}).");
            }

            return value;
        }
    }

    /// <summary>
    /// Pure battle logic for RPG/quiz fights.
    /// </summary>
    public sealed class QuizBattleEngine
    {
        private static readonly Dictionary<int, EnemyDifficultyProfile> _difficultyByLevel = new()
        {
            { 1, new EnemyDifficultyProfile(1, "Easy", 1) },
            { 2, new EnemyDifficultyProfile(2, "Normal", 2) },
            { 3, new EnemyDifficultyProfile(3, "Hard", 3) },
            { 4, new EnemyDifficultyProfile(4, "Very Hard", 4) },
            { 5, new EnemyDifficultyProfile(5, "Boss", 5) }
        };
        // Keep total player card damage at 100 when all 5 cards are used once.
        private static readonly int[] _defaultCardDamages = { 10, 15, 20, 25, 30 };
        private const int SkipAllQuestionsDamage = 100;

        private readonly IRandomProvider _randomProvider;
        private readonly Dictionary<int, HashSet<int>> _enemyCardKeyNumbers = new();
        private readonly List<AttackCard> _attackCards = new();

        public int PlayerHP { get; private set; } = 100;
        public int EnemyHP { get; private set; } = 100;
        public int? LockedCardId { get; private set; }
        public EnemyDifficultyProfile Difficulty { get; }
        public IReadOnlyList<AttackCard> AttackCards => _attackCards;

        public void ApplyChipDamageToPlayer(int amount)
        {
            PlayerHP = Math.Max(0, PlayerHP - amount);
        }

        public QuizBattleEngine(int enemyLevel, IRandomProvider? randomProvider = null, Dictionary<int, HashSet<int>>? fixedEnemyCardKeyNumbers = null)
        {
            if (!_difficultyByLevel.TryGetValue(enemyLevel, out var difficulty))
            {
                throw new ArgumentOutOfRangeException(nameof(enemyLevel), "Enemy level must be in range 1..5.");
            }

            Difficulty = difficulty;
            _randomProvider = randomProvider ?? new DefaultRandomProvider();
            InitializeAttackCards();

            if (fixedEnemyCardKeyNumbers != null)
            {
                _enemyCardKeyNumbers = fixedEnemyCardKeyNumbers.ToDictionary(
                    kvp => kvp.Key,
                    kvp => new HashSet<int>(kvp.Value));
            }
            else
            {
                BuildEnemyCardKeyNumbers();
            }
        }

        private void InitializeAttackCards()
        {
            if (_defaultCardDamages.Sum() != 100)
            {
                throw new InvalidOperationException("Card damage configuration is invalid. Total of 5 cards must be exactly 100.");
            }

            _attackCards.Clear();
            for (int i = 0; i < _defaultCardDamages.Length; i++)
            {
                _attackCards.Add(new AttackCard(i + 1, _defaultCardDamages[i]));
            }
        }

        /// <summary>
        /// True when player can choose this card based on lock rules.
        /// </summary>
        public bool CanSelectCard(int cardId)
        {
            return LockedCardId == null || LockedCardId.Value == cardId;
        }

        /// <summary>
        /// Runs one player turn, including enemy retaliation when answer is wrong.
        /// </summary>
        public PlayerTurnResult PlayerTurn(int selectedCardId, bool answerCorrect)
        {
            AttackCard selectedCard = GetAttackCard(selectedCardId);

            if (!CanSelectCard(selectedCardId))
            {
                return CreateBlockedTurnResult(selectedCardId);
            }

            if (answerCorrect)
            {
                EnemyHP = Math.Max(0, EnemyHP - selectedCard.Damage);
                LockedCardId = null;

                return new PlayerTurnResult
                {
                    SelectedCardId = selectedCardId,
                    AnswerCorrect = true,
                    PlayerAttacked = true,
                    DamageToEnemy = selectedCard.Damage,
                    CardLocked = false,
                    LockedCardId = null,
                    RetryRequired = false,
                    Message = $"Correct answer. Card {selectedCardId} dealt {selectedCard.Damage} damage.",
                    BattleResult = CheckBattleResult()
                };
            }

            // Wrong answer: lock this card and start enemy attack sequence.
            LockedCardId = selectedCardId;
            var enemyAttacks = EnemyAttack();

            return new PlayerTurnResult
            {
                SelectedCardId = selectedCardId,
                AnswerCorrect = false,
                PlayerAttacked = false,
                DamageToEnemy = 0,
                CardLocked = true,
                LockedCardId = LockedCardId,
                RetryRequired = true,
                Message = $"Wrong answer. Card {selectedCardId} is locked. Replace/reshuffle question and retry this same card.",
                BattleResult = CheckBattleResult(),
                EnemyAttacks = enemyAttacks
            };
        }

        public PlayerTurnResult SkipCurrentQuestion(int selectedCardId)
        {
            AttackCard selectedCard = GetAttackCard(selectedCardId);

            if (!CanSelectCard(selectedCardId))
            {
                return CreateBlockedTurnResult(selectedCardId);
            }

            EnemyHP = Math.Max(0, EnemyHP - selectedCard.Damage);
            LockedCardId = null;

            return new PlayerTurnResult
            {
                SelectedCardId = selectedCardId,
                AnswerCorrect = false,
                PlayerAttacked = true,
                DamageToEnemy = selectedCard.Damage,
                CardLocked = false,
                LockedCardId = null,
                RetryRequired = false,
                Message = $"Skipped question. Card {selectedCardId} dealt {selectedCard.Damage} damage.",
                BattleResult = CheckBattleResult(),
                SkipCommand = QuestionSkipCommandType.SkipCurrentQuestion
            };
        }

        public PlayerTurnResult SkipAllRemainingQuestions(int selectedCardId)
        {
            GetAttackCard(selectedCardId);

            if (!CanSelectCard(selectedCardId))
            {
                return CreateBlockedTurnResult(selectedCardId);
            }

            EnemyHP = Math.Max(0, EnemyHP - SkipAllQuestionsDamage);
            LockedCardId = null;

            return new PlayerTurnResult
            {
                SelectedCardId = selectedCardId,
                AnswerCorrect = false,
                PlayerAttacked = true,
                DamageToEnemy = SkipAllQuestionsDamage,
                CardLocked = false,
                LockedCardId = null,
                RetryRequired = false,
                Message = $"Skipped remaining questions. Attack dealt {SkipAllQuestionsDamage} damage.",
                BattleResult = CheckBattleResult(),
                SkipCommand = QuestionSkipCommandType.SkipAllQuestions
            };
        }

        private AttackCard GetAttackCard(int selectedCardId)
        {
            var selectedCard = _attackCards.FirstOrDefault(c => c.Id == selectedCardId);
            if (selectedCard == null)
            {
                throw new ArgumentOutOfRangeException(nameof(selectedCardId), "Card id must be 1..5.");
            }

            return selectedCard;
        }

        private PlayerTurnResult CreateBlockedTurnResult(int selectedCardId)
        {
            return new PlayerTurnResult
            {
                SelectedCardId = selectedCardId,
                AnswerCorrect = false,
                PlayerAttacked = false,
                DamageToEnemy = 0,
                CardLocked = true,
                LockedCardId = LockedCardId,
                RetryRequired = true,
                Message = $"Card {LockedCardId} is locked. Retry the locked card first.",
                BattleResult = CheckBattleResult()
            };
        }

        /// <summary>
        /// Enemy attacks only when the player answers incorrectly.
        /// It can chain attacks if the drawn trigger number matches key numbers of the drawn card.
        /// </summary>
        public IReadOnlyList<EnemyAttackEvent> EnemyAttack()
        {
            List<EnemyAttackEvent> attacks = new();

            bool continueAttacking = true;
            while (continueAttacking && PlayerHP > 0 && EnemyHP > 0)
            {
                // Step 1: enemy draws one random card from the same five cards.
                int cardIndex = _randomProvider.Next(0, _attackCards.Count);
                AttackCard enemyCard = _attackCards[cardIndex];

                // Step 2: apply card damage to the player.
                PlayerHP = Math.Max(0, PlayerHP - enemyCard.Damage);

                // Step 3: fresh number draw from 1..10 for each attack cycle.
                int triggerNumber = _randomProvider.Next(1, 11);
                bool triggersAnotherAttack = _enemyCardKeyNumbers[enemyCard.Id].Contains(triggerNumber);

                attacks.Add(new EnemyAttackEvent(enemyCard.Id, enemyCard.Damage, triggerNumber, triggersAnotherAttack));
                continueAttacking = triggersAnotherAttack;
            }

            return attacks;
        }

        /// <summary>
        /// Checks current HP and returns battle result.
        /// </summary>
        public BattleResult CheckBattleResult()
        {
            if (PlayerHP <= 0) return BattleResult.PlayerDefeat;
            if (EnemyHP <= 0) return BattleResult.EnemyDefeat;
            return BattleResult.Ongoing;
        }

        /// <summary>
        /// Creates card key numbers based on enemy level.
        /// Each card gets unique numbers from 1..10.
        /// </summary>
        private void BuildEnemyCardKeyNumbers()
        {
            _enemyCardKeyNumbers.Clear();
            foreach (var card in _attackCards)
            {
                List<int> pool = Enumerable.Range(1, 10).ToList();
                HashSet<int> keys = new();

                for (int i = 0; i < Difficulty.KeyNumbersPerCard; i++)
                {
                    int index = _randomProvider.Next(0, pool.Count);
                    keys.Add(pool[index]);
                    pool.RemoveAt(index);
                }

                _enemyCardKeyNumbers[card.Id] = keys;
            }
        }

        /// <summary>
        /// Simple deterministic simulation for manual verification.
        /// Demonstrates:
        /// 1) player selects card,
        /// 2) correct answer attack,
        /// 3) wrong answer,
        /// 4) enemy attacks,
        /// 5) locked-card retry behavior.
        /// </summary>
        public static IReadOnlyList<string> RunSimpleTestSimulation()
        {
            var fixedKeys = new Dictionary<int, HashSet<int>>
            {
                { 1, new HashSet<int> { 2, 9 } },
                { 2, new HashSet<int> { 4, 7 } },
                { 3, new HashSet<int> { 1, 8 } },
                { 4, new HashSet<int> { 5, 10 } },
                { 5, new HashSet<int> { 3, 6 } }
            };

            // Sequence for enemy attack phase:
            // Draw card index 4 (card 5), trigger=3 (chain),
            // draw card index 0 (card 1), trigger=10 (stop).
            var random = new QueueRandomProvider(new[] { 4, 3, 0, 10 });
            var engine = new QuizBattleEngine(enemyLevel: 2, randomProvider: random, fixedEnemyCardKeyNumbers: fixedKeys);

            List<string> log = new()
            {
                "SIM START -> PlayerHP=100, EnemyHP=100"
            };

            // 1 + 2. Player selects card 3 and answers correctly.
            var turn1 = engine.PlayerTurn(selectedCardId: 3, answerCorrect: true);
            log.Add($"Turn1: Selected card {turn1.SelectedCardId}, correct={turn1.AnswerCorrect}, enemyHP={engine.EnemyHP}");

            // 3 + 4. Player selects card 2 and answers incorrectly, enemy attacks.
            var turn2 = engine.PlayerTurn(selectedCardId: 2, answerCorrect: false);
            log.Add($"Turn2: Selected card {turn2.SelectedCardId}, correct={turn2.AnswerCorrect}, lockedCard={turn2.LockedCardId}");
            foreach (var enemyAttack in turn2.EnemyAttacks)
            {
                log.Add($"EnemyAttack: card={enemyAttack.CardId}, damage={enemyAttack.Damage}, draw={enemyAttack.TriggerNumber}, chain={enemyAttack.TriggersAnotherAttack}");
            }
            log.Add($"After enemy attacks -> PlayerHP={engine.PlayerHP}");

            // 5. Player tries different card while locked (blocked), then retries locked card correctly.
            var blockedTurn = engine.PlayerTurn(selectedCardId: 4, answerCorrect: true);
            log.Add($"LockedCheck: attempt card {blockedTurn.SelectedCardId}, attacked={blockedTurn.PlayerAttacked}, message='{blockedTurn.Message}'");

            var retryTurn = engine.PlayerTurn(selectedCardId: 2, answerCorrect: true);
            log.Add($"RetryTurn: selected card {retryTurn.SelectedCardId}, correct={retryTurn.AnswerCorrect}, lockedCard={(engine.LockedCardId?.ToString() ?? "none")}, enemyHP={engine.EnemyHP}");

            log.Add($"SIM END -> Result={engine.CheckBattleResult()}, PlayerHP={engine.PlayerHP}, EnemyHP={engine.EnemyHP}");
            return log;
        }
    }
}
