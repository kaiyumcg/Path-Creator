# Path-Creator
Path creation asset for Unity game development

The manual is [here](https://github.com/SebLague/Path-Creator#readme)
#### Installation:
Add an entry in your manifest.json as follows:
```C#
"com.kaiyum.pathcreator": "https://github.com/kaiyumcg/Path-Creator.git"
```

Since unity does not support git dependencies, you need the following entries as well:
```C#
"com.kaiyum.attributeext" : "https://github.com/kaiyumcg/AttributeExt.git",
"com.kaiyum.unityext": "https://github.com/kaiyumcg/UnityExt.git",
"com.kaiyum.editorutil": "https://github.com/kaiyumcg/EditorUtil.git"
```
Add them into your manifest.json file in "Packages\" directory of your unity project, if they are already not in manifest.json file.