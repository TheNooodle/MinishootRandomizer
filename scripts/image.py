#!/usr/bin/python3

# This script adjusts the width and height of an image to be multiples of 4,
# while maintaining the original aspect ratio as closely as possible.
# This is because Unity requires textures to be in multiples of 4
# cf FileSpriteProvider.cs

def adjust_multiple_4(width, height):
    ratio = width / height
    
    # Option 1: fixer largeur
    w1 = round(width/4) * 4
    h1 = round(w1/ratio/4) * 4
    
    # Option 2: fixer hauteur  
    h2 = round(height/4) * 4
    w2 = round(h2*ratio/4) * 4
    
    # Choisir l'option avec le moins d'écart de ratio
    ratio1 = abs(w1/h1 - ratio)
    ratio2 = abs(w2/h2 - ratio)
    
    return (w1, h1) if ratio1 <= ratio2 else (w2, h2)

print("Enter width and height (e.g., 1920 1080):")
width, height = map(int, input().split())
adjusted_width, adjusted_height = adjust_multiple_4(width, height)
print(f"Adjusted dimensions: {adjusted_width}x{adjusted_height}")
print(f"Aspect ratio: {adjusted_width / adjusted_height:.2f}")
