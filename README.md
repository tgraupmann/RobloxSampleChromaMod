# Roblox Sample Chroma RGB Mod

This mod uses print statements and a companion app to monitor the game log and play Chroma RGB effects.


## Quick Start ##

1. Open [RobloxSampleChromaMod.rbxl](RobloxSampleChromaMod.rbxl)

2. Hit Play!

3. Launch `WPF_RobloxChromaMod` which monitors the game log and plays `Chroma RGB` for the various game events.


## Videos ##


**Chroma RGB in Roblox (Overview)**

<a target="_blank" href="https://youtu.be/AI5I4I07aW8"><img src="https://img.youtube.com/vi/AI5I4I07aW8/0.jpg"></a>


**Create Chroma Animations with the [Web Chroma Editor](https://chroma.razer.com/ChromaEditor/)**

<a target="_blank" href="https://youtu.be/ZVX3DNW2gIM"><img src="https://img.youtube.com/vi/ZVX3DNW2gIM/0.jpg"></a>


**Screenshot**

**Roblox**

![image_1](images/image_1.png)

**WPF_RobloxChromaMod**

![image_3](images/image_3.png)


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
		if (string.len(state) > tokenLength) then
			local strState = string.sub(state, tokenLength)
			print ("ChromaRGB:", "Player_", strState);
		end
	end
end)
```

## Support

Contact `Tim Graupmann#0611` on Discord for support.
