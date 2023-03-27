-- globals

-- button effects
_G.ChromaEffect = 0;

-- game state
_G.GameStateClimbing = false
_G.GameStateJumping = false
_G.GameStateFlying = false
_G.GameStateRunning = 0
_G.GameStateSwimming = true
_G.GameStateSeated = false

_G.GameStateTextLabel = script.Parent

while wait(0.033) do
	
	-- button state
	red = 0
	if (_G.ChromaEffect == "BtnEffectNone") then
		red = 0
	elseif (_G.ChromaEffect == "BtnEffect1") then
		red = 1
	elseif (_G.ChromaEffect == "BtnEffect2") then
		red = 2
	elseif (_G.ChromaEffect == "BtnEffect3") then
		red = 3
	elseif (_G.ChromaEffect == "BtnEffect4") then
		red = 4
	elseif (_G.ChromaEffect == "BtnEffect5") then
		red = 5
	elseif (_G.ChromaEffect == "BtnEffect6") then
		red = 6
	elseif (_G.ChromaEffect == "BtnEffect7") then
		red = 7
	elseif (_G.ChromaEffect == "BtnEffect8") then
		red = 8
	elseif (_G.ChromaEffect == "BtnEffect9") then
		red = 9
	elseif (_G.ChromaEffect == "BtnEffect10") then
		red = 10
	elseif (_G.ChromaEffect == "BtnEffect11") then
		red = 11
	elseif (_G.ChromaEffect == "BtnEffect12") then
		red = 12
	elseif (_G.ChromaEffect == "BtnEffect13") then
		red = 13
	elseif (_G.ChromaEffect == "BtnEffect14") then
		red = 14
	elseif (_G.ChromaEffect == "BtnEffect15") then
		red = 15
	end
	
	-- game state
	green = 0
	if _G.GameStateDead then
		green = 1
	else
		if _G.GameStateClimbing then
			green = bit32.bor(green, bit32.lshift(1, 1))
		end
		if _G.GameStateJumping then
			green = bit32.bor(green, bit32.lshift(1, 2))
		end
		if _G.GameStateFlying then
			green = bit32.bor(green, bit32.lshift(1, 3))
		end
		if _G.GameStateRunning > os.time() then
			green = bit32.bor(green, bit32.lshift(1, 4))
		end
		if _G.GameStateSwimming then
			green = bit32.bor(green, bit32.lshift(1, 5))
		end
		if _G.GameStateSeated then
			green = bit32.bor(green, bit32.lshift(1, 6))
		end
	end
	
	-- final color
	script.Parent.BackgroundColor3 = Color3.fromRGB(red, green, 0)
end