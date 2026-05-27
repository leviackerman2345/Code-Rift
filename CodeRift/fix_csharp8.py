import os
import re

def fix_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    original = content

    # 1. Nullable reference types: Type? -> Type
    # Common ones: byte[]?, Image?[], Action?, Task?, Control?, IRandomProvider?, Image?
    content = re.sub(r'\b(byte\[\])\?', r'\1', content)
    content = re.sub(r'\b(Image|Action|Task|Control|IRandomProvider)\[\]\?', r'\1[]', content)
    content = re.sub(r'\b(Image|Action|Task|Control|IRandomProvider|string|object)\?', r'\1', content)

    # 2. Target-typed new: new() -> new Type()
    # e.g., PrewarmTasks = new(); -> PrewarmTasks = new Dictionary<int, Task>();
    content = re.sub(r'public\s+Dictionary<int,\s*Task>\s+PrewarmTasks\s*\{\s*get;\s*\}\s*=\s*new\(\);', r'public Dictionary<int, Task> PrewarmTasks { get; } = new Dictionary<int, Task>();', content)
    content = re.sub(r'public\s+List<Question>\s+Questions\s*\{\s*get;\s*set;\s*\}\s*=\s*new\(\);', r'public List<Question> Questions { get; set; } = new List<Question>();', content)

    # 3. Add namespaces to Designer.cs
    if filepath.endswith('.Designer.cs'):
        if 'using System.Drawing;' not in content:
            content = 'using System.Drawing;\n' + content
        if 'using System.Windows.Forms;' not in content:
            content = 'using System.Windows.Forms;\n' + content

    if content != original:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)

for root, dirs, files in os.walk('.'):
    for f in files:
        if f.endswith('.cs'):
            fix_file(os.path.join(root, f))
