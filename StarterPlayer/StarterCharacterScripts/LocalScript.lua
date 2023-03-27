local character = script.Parent

local humanoid = character:WaitForChild("Humanoid")

local tokenLength = string.len("Enum.HumanoidStateType.")

-- listen to humanoid state
humanoid.StateChanged:Connect(function(oldState, newState)
	if (newState ~= nil and oldState ~= newState and newState ~= Enum.HumanoidStateType.None) then
		local state = tostring(newState);
		if (string.len(state) > tokenLength) then
			local strState = string.sub(state, tokenLength + 1)
			--print ("ChromaRGB:", string.format("Player_%s", strState))
			if (strState == "Dead") then
				_G.GameStateDead = true
				_G.GameStateClimbing = false
				_G.GameStateJumping = false
				_G.GameStateFlying = false
				_G.GameStateRunning = 0
				_G.GameStateSwimming = false
				_G.GameStateSeated = false
			elseif strState == "Climbing" then
				_G.GameStateDead = false
				_G.GameStateClimbing = true
				_G.GameStateJumping = false
				_G.GameStateFlying = false
				_G.GameStateRunning = 0
				_G.GameStateSwimming = false
				_G.GameStateSeated = false
			elseif strState == "Jumping" then
				_G.GameStateDead = false
				_G.GameStateClimbing = false
				_G.GameStateJumping = true
				_G.GameStateFlying = false
				_G.GameStateRunning = 0
				_G.GameStateSwimming = false
				_G.GameStateSeated = false
			elseif strState == "Flying" then
				_G.GameStateDead = false
				_G.GameStateClimbing = false
				_G.GameStateJumping = false
				_G.GameStateFlying = true
				_G.GameStateRunning = 0
				_G.GameStateSwimming = false
				_G.GameStateSeated = false
			elseif strState == "Landed" then
				_G.GameStateDead = false
				_G.GameStateClimbing = false
				_G.GameStateJumping = false
				_G.GameStateFlying = false
				_G.GameStateRunning = 0
				_G.GameStateSwimming = false
				_G.GameStateSeated = false
			elseif strState == "Running" then
				_G.GameStateDead = false
				_G.GameStateClimbing = false
				_G.GameStateJumping = false
				_G.GameStateFlying = false
				_G.GameStateRunning = os.time() + 2
				_G.GameStateSwimming = false
				_G.GameStateSeated = false
			elseif strState == "Seated" then
				_G.GameStateDead = false
				_G.GameStateClimbing = false
				_G.GameStateJumping = false
				_G.GameStateFlying = false
				_G.GameStateRunning = 0
				_G.GameStateSwimming = false
				_G.GameStateSeated = true
			elseif strState == "Swimming" then
				_G.GameStateDead = false
				_G.GameStateClimbing = false
				_G.GameStateJumping = false
				_G.GameStateFlying = false
				_G.GameStateRunning = 0
				_G.GameStateSwimming = true
				_G.GameStateSeated = false
			elseif strState == "WASD" then
				if (not _G.GameStateDead and
					not _G.GameStateClimbing and
					not _G.GameStateSwimming and
					not _G.GameStateFlying) then
					_G.GameStateRunning = os.time() + 2
				end
			end
		end
	end
end)


local UserInputService = game:GetService("UserInputService")

local function onInputBegan(inputObject, gameProcessedEvent)
	-- First check if the "gameProcessedEvent" is true
	-- This indicates that another script had already processed the input, so this one can be ignored
	if gameProcessedEvent then return end
	-- Next, check that the input was a keyboard event
	if inputObject.UserInputType == Enum.UserInputType.Keyboard then
		if (not _G.GameStateDead) then
			if (not _G.GameStateClimbing and
				not _G.GameStateSwimming and
				not _G.GameStateSeated) then
				if (not _G.GameStateFlying and
					not _G.GameStateJumping and
					inputObject.KeyCode.Name == "W" or
					inputObject.KeyCode.Name == "A" or
					inputObject.KeyCode.Name == "S" or
					inputObject.KeyCode.Name == "D") then
					--print("Time:", os.time(), "ChromaRGB:", "Player_WASD")
					_G.GameStateRunning = os.time() + 2
				elseif (inputObject.KeyCode.Name == "F") then
					_G.GameStateFlying = not _G.GameStateFlying			
				end
			end
		end
		
	end
end

UserInputService.InputBegan:Connect(onInputBegan)
