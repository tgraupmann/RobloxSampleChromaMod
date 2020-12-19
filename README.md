# RobloxSampleChromaMod

Sample Chroma RGB Mod for Roblox

## Videos ##

**Chroma RGB in Roblox (Overview)**

<a target="_blank" href="https://youtu.be/AI5I4I07aW8"><img src="https://img.youtube.com/vi/AI5I4I07aW8/0.jpg"></a>

**Screenshot**

**Roblox**

![image_1](images/image_1.png)

**WPF_RobloxChromaMod**

![image_3](images/image_3.png)

## Quick Start ##

1. Open [RobloxSampleChromaMod.rbxl](RobloxSampleChromaMod.rbxl)

2. Hit Play!

3. Launch `WPF_RobloxChromaMod` which monitors the game log and plays `Chroma RGB` for the various game events.

## Scripts ##


**TextButton Script**

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


**Player State Script**

Print player state events so that Chroma can react to changes. [StarterPlayer/StarterCharacterScripts/LocalScript.lua](StarterPlayer/StarterCharacterScripts/LocalScript.lua)

```lua
local character = script.Parent

local humanoid = character:WaitForChild("Humanoid")

local tokenLength = 24 -- "Enum.HumanoidStateType."

-- listen to humanoid state
humanoid.StateChanged:Connect(function(oldState, newState)
	if (newState ~= nil and oldState ~= newState and newState ~= Enum.HumanoidStateType.None) then
		local state = tostring(newState);
		-- print ("Player ", state)
		if (string.len(state) > tokenLength) then
			local strState = string.sub(state, tokenLength)
			print (ChromaRGB:", "Player_", strState);
		end
	end
end)
```
