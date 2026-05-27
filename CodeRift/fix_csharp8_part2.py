import os
import re

def fix_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    original = content

    # Nullable reference types to strip: Type?, Type?[]
    # But only for specific known reference types to avoid breaking bool?, int?, float?, decimal?
    ref_types = ['string', 'object', 'Image', 'Action', 'Task', 'Control', 'Label', 'PictureBox', 'Panel', 'Question', 'IRandomProvider', 'Rectangle', 'Dictionary', 'Action<int>', 'Action<string>']
    
    for t in ref_types:
        # Match 'Type?' -> 'Type' (be careful of word boundaries)
        # e.g. string? -> string
        content = re.sub(r'\b' + re.escape(t) + r'\?', t, content)
        # Match 'Type?[]' -> 'Type[]'
        content = re.sub(r'\b' + re.escape(t) + r'\?\[\]', t + '[]', content)

    # Dictionary<,> missing in ImageManager and LanguageManager
    if 'using System.Collections.Generic;' not in content and 'Dictionary' in content:
        content = 'using System.Collections.Generic;\n' + content

    if 'Rectangle' in content and 'using System.Drawing;' not in content:
        content = 'using System.Drawing;\n' + content

    if content != original:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)

for root, dirs, files in os.walk('.'):
    for f in files:
        if f.endswith('.cs'):
            fix_file(os.path.join(root, f))
