import os
import re

filepath = 'Core/AssetBootstrapper.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# Replace (string Key, string Path)[] ImageAssets = { (..., ...), ... }
# We will define a struct AssetDef { public string Key; public string Path; public AssetDef(string k, string p) { Key=k; Path=p; } }
struct_def = '''
    public struct AssetDef
    {
        public string Key;
        public string Path;
        public AssetDef(string key, string path) { Key = key; Path = path; }
    }
'''

content = content.replace('public static class AssetBootstrapper\n    {', 'public static class AssetBootstrapper\n    {' + struct_def)

# Replace (string Key, string Path)[]
content = content.replace('private static readonly (string Key, string Path)[] ImageAssets =', 'private static readonly AssetDef[] ImageAssets =')
content = content.replace('private static readonly (string Key, string Path)[] AudioAssets =', 'private static readonly AssetDef[] AudioAssets =')

# Replace tuple instantiations: (Constants.IMG_BG_MENU, Path.Combine(...))
# This is tricky because of nested parentheses. Let's do it simply line by line.
lines = content.split('\n')
for i, line in enumerate(lines):
    if line.strip().startswith('(') and line.strip().endswith('),'):
        # Just replace the starting '(' with 'new AssetDef('
        lines[i] = line.replace('(', 'new AssetDef(', 1)
    elif line.strip().startswith('(') and line.strip().endswith(')'):
        # Last element in array
        lines[i] = line.replace('(', 'new AssetDef(', 1)

content = '\n'.join(lines)

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)
print('Fixed AssetBootstrapper.cs')
