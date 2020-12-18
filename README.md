# RobloxSampleChromaMod

Sample Chroma RGB Mod for Roblox

**Screenshot**

![image_1](images/image_1.png)

## Quick Start ##

1. Open [RobloxSampleChromaMod.rbxl](RobloxSampleChromaMod.rbxl)

2. Hit Play!

## Scripts ##

Notice that `LocalScript` is a child of the `TextButton` at: [StarterGui/ChromaGui/Frame/BtnEffect1/LocalScript.lua](StarterGui/ChromaGui/Frame/BtnEffect1/LocalScript.lua)

![image_2](images/image_2.png)

The script prints `ChromaRGB: BtnEffectN` to the log which is monitored to play the corresponding Chroma RGB effect.

```lua
function leftClick()
	print("ChromaRGB:", script.Parent.Name)
end

function rightClick()
	print("ChromaRGB:", script.Parent.Name)
end

script.Parent.MouseButton1Click:Connect(leftClick)
script.Parent.MouseButton2Click:Connect(rightClick)
```
