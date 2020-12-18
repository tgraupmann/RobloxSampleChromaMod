function leftClick()
	print("ChromaRGB:", script.Parent.Name)
end

function rightClick()
	print("ChromaRGB:", script.Parent.Name)
end

script.Parent.MouseButton1Click:Connect(leftClick)
script.Parent.MouseButton2Click:Connect(rightClick)
