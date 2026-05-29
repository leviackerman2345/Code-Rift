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
        public int Id { get; private set; }
        public int Damage { get; private set; }

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
        public int Level { get; private set; }
        public string Name { get; private set; }
        public int KeyNumbersPerCard { get; private set; }

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
        public int CardId { get; private set; }
        public int Damage { get; private set; }
        public int TriggerNumber { get; private set; }
        public bool TriggersAnotherAttack { get; private set; }

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
        public PlayerTurnResult()
        {
            Message = string.Empty;
            EnemyAttacks = new List<EnemyAttackEvent>();
        }

        public int SelectedCardId { get; set; }
        public bool AnswerCorrect { get; set; }
        public bool PlayerAttacked { get; set; }
        public int DamageToEnemy { get; set; }
        public bool CardLocked { get; set; }
        public int? LockedCardId { get; set; }
        public bool RetryRequired { get; set; }
        public string Message { get; set; }
        public BattleResult BattleResult { get; set; }
        public QuestionSkipCommandType SkipCommand { get; set; }
        public List<EnemyAttackEvent> EnemyAttacks { get; set; }
    }

    public interface IRandomProvider
    {
        int Next(int minValueInclusive, int maxValueExclusive);
    }

    public sealed class DefaultRandomProvider : IRandomProvider
    {
        private static readonly Random _random = new Random();

        public int Next(int minValueInclusive, int maxValueExclusive)
        {
            return _random.Next(minValueInclusive, maxValueExclusive);
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
                throw new ArgumentOutOfRangeException("value", string.Format("Value {0} is outside [{1}, {2}).", value, minValueInclusive, maxValueExclusive));
            }

            return value;
        }
    }

    /// <summary>
    /// Pure battle logic for RPG/quiz fights.
    /// </summary>
    public sealed class QuizBattleEngine
    {
        private static readonly Dictionary<int, EnemyDifficultyProfile> _difficultyByLevel = new Dictionary<int, EnemyDifficultyProfile>()
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
        private readonly Dictionary<int, HashSet<int>> _enemyCardKeyNumbers = new Dictionary<int, HashSet<int>>();
        private readonly List<AttackCard> _attackCards = new List<AttackCard>();

        public int PlayerHP { get; private set; }
        public int EnemyHP { get; private set; }
        public int? LockedCardId { get; private set; }
        public EnemyDifficultyProfile Difficulty { get; private set; }
        public List<AttackCard> AttackCards { get { return _attackCards; } }

        public void ApplyChipDamageToPlayer(int amount)
        {
            PlayerHP = Math.Max(0, PlayerHP - amount);
        }

        public QuizBattleEngine(int enemyLevel, IRandomProvider randomProvider = null, Dictionary<int, HashSet<int>> fixedEnemyCardKeyNumbers = null)
        {
            PlayerHP = 100;
            EnemyHP = 100;
            EnemyDifficultyProfile difficulty;
            if (!_difficultyByLevel.TryGetValue(enemyLevel, out difficulty))
            {
                throw new ArgumentOutOfRangeException("enemyLevel", "Enemy level must be in range 1..5.");
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
                    Message = string.Format("Correct answer. Card {0} dealt {1} damage.", selectedCardId, selectedCard.Damage),
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
                Message = string.Format("Wrong answer. Card {0} is locked. Replace/reshuffle question and retry this same card.", selectedCardId),
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
                Message = string.Format("Skipped question. Card {0} dealt {1} damage.", selectedCardId, selectedCard.Damage),
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
                Message = string.Format("Skipped remaining questions. Attack dealt {0} damage.", SkipAllQuestionsDamage),
                BattleResult = CheckBattleResult(),
                SkipCommand = QuestionSkipCommandType.SkipAllQuestions
            };
        }

        private AttackCard GetAttackCard(int selectedCardId)
        {
            var selectedCard = _attackCards.FirstOrDefault(c => c.Id == selectedCardId);
            if (selectedCard == null)
            {
                throw new ArgumentOutOfRangeException("selectedCardId", "Card id must be 1..5.");
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
                Message = string.Format("Card {0} is locked. Retry the locked card first.", LockedCardId),
                BattleResult = CheckBattleResult()
            };
        }

        /// <summary>
        /// Enemy attacks only when the player answers incorrectly.
        /// It can chain attacks if the drawn trigger number matches key numbers of the drawn card.
        /// </summary>
        public List<EnemyAttackEvent> EnemyAttack()
        {
            List<EnemyAttackEvent> attacks = new List<EnemyAttackEvent>();

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
                HashSet<int> keys = new HashSet<int>();

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
        public static List<string> RunSimpleTestSimulation()
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

            List<string> log = new List<string>()
            {
                "SIM START -> PlayerHP=100, EnemyHP=100"
            };

            // 1 + 2. Player selects card 3 and answers correctly.
            var turn1 = engine.PlayerTurn(selectedCardId: 3, answerCorrect: true);
            log.Add(string.Format("Turn1: Selected card {0}, correct={1}, enemyHP={2}", turn1.SelectedCardId, turn1.AnswerCorrect, engine.EnemyHP));

            // 3 + 4. Player selects card 2 and answers incorrectly, enemy attacks.
            var turn2 = engine.PlayerTurn(selectedCardId: 2, answerCorrect: false);
            log.Add(string.Format("Turn2: Selected card {0}, correct={1}, lockedCard={2}", turn2.SelectedCardId, turn2.AnswerCorrect, turn2.LockedCardId));
            foreach (var enemyAttack in turn2.EnemyAttacks)
            {
                log.Add(string.Format("EnemyAttack: card={0}, damage={1}, draw={2}, chain={3}", enemyAttack.CardId, enemyAttack.Damage, enemyAttack.TriggerNumber, enemyAttack.TriggersAnotherAttack));
            }
            log.Add(string.Format("After enemy attacks -> PlayerHP={0}", engine.PlayerHP));

            // 5. Player tries different card while locked (blocked), then retries locked card correctly.
            var blockedTurn = engine.PlayerTurn(selectedCardId: 4, answerCorrect: true);
            log.Add(string.Format("LockedCheck: attempt card {0}, attacked={1}, message='{2}'", blockedTurn.SelectedCardId, blockedTurn.PlayerAttacked, blockedTurn.Message));

            var retryTurn = engine.PlayerTurn(selectedCardId: 2, answerCorrect: true);
            log.Add(string.Format("RetryTurn: selected card {0}, correct={1}, lockedCard={2}, enemyHP={3}", retryTurn.SelectedCardId, retryTurn.AnswerCorrect, engine.LockedCardId != null ? engine.LockedCardId.ToString() : "none", engine.EnemyHP));

            log.Add(string.Format("SIM END -> Result={0}, PlayerHP={1}, EnemyHP={2}", engine.CheckBattleResult(), engine.PlayerHP, engine.EnemyHP));
            return log;
        }
    }
}
