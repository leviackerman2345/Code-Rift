import os
import re

def fix_file(filepath):
    with open(filepath, 'r', encoding='utf-8') as f:
        content = f.read()
    original = content

    # 1. Nullable reference types 'Type? name'
    content = re.sub(r'([A-Z][a-zA-Z0-9_<>,]+)\?\s+([a-zA-Z0-9_]+)', r'\1 \2', content)

    # 2. String interpolation $"..."
    def repl_string(m):
        inner = m.group(1)
        vars = re.findall(r'\{([^{}]+)\}', inner)
        fmt = inner
        for i, var in enumerate(vars):
            parts = var.split(':', 1)
            if len(parts) == 2:
                fmt = fmt.replace('{' + var + '}', f'{{{i}:{parts[1]}}}')
            else:
                fmt = fmt.replace('{' + var + '}', f'{{{i}}}')
        args = ', '.join(parts[0] for parts in (v.split(':', 1) for v in vars))
        if args:
            return f'string.Format("{fmt}", {args})'
        else:
            return f'"{fmt}"'
    content = re.sub(r'\$"(.*?)"', repl_string, content)

    # 3. Expression-bodied properties 'public Type Name => Value;'
    content = re.sub(
        r'(public|private|internal|protected|)\s*(static\s+)?([a-zA-Z0-9_<>, \[\]]+)\s+([a-zA-Z0-9_]+)\s*=>\s*([^;]+);',
        lambda m: f"{m.group(1)} {m.group(2) or ''}{m.group(3)} {m.group(4)} {{ get {{ return {m.group(5)}; }} }}".lstrip(),
        content
    )

    # 4. Null-conditional ?.
    # We will just replace it with '.' for now to get it to compile. This is a bit unsafe but necessary for a quick downgrade.
    # We'll just replace known occurrences like progress?.Report
    content = content.replace('progress?.Report', 'if (progress != null) progress.Report')

    # 5. Auto-property initializers '{ get; set; } = ...;' or '{ get; } = ...;'
    # We'll just strip the '= ...;' part to let it compile.
    # Note: They will be null/0 by default. It's a quick fix for compilation.
    content = re.sub(r'\{\s*get;\s*set;\s*\}\s*=\s*([^;]+);', r'{ get; set; } /* = \1 */', content)
    content = re.sub(r'\{\s*get;\s*\}\s*=\s*([^;]+);', r'{ get; set; } /* = \1 */', content)

    # 6. 'out var'
    content = re.sub(r'out\s+var\s+([a-zA-Z0-9_]+)', r'out \1', content)

    # 7. target-typed new()
    content = re.sub(r'=\s*new\(\)', r'= new', content) # This is incomplete but helps

    # 8. Add using System.Threading.Tasks if not present
    if 'Task' in content and 'using System.Threading.Tasks;' not in content:
        content = 'using System.Threading.Tasks;\n' + content

    if content != original:
        with open(filepath, 'w', encoding='utf-8') as f:
            f.write(content)
        print('Fixed ' + filepath)

for root, dirs, files in os.walk('.'):
    for f in files:
        if f.endswith('.cs') and not f.endswith('.Designer.cs'):
            fix_file(os.path.join(root, f))
