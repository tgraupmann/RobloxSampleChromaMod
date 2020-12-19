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
			print ("ChromaRGB:", "Player_", strState);
		end
	end
end)
