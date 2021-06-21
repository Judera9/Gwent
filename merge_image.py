from PIL import Image
import os

img_list = list()

for i in range(10):
	img_list.append(Image.open('../../Alice-1-%d_1.png' % (i+1)))
	os.remove('../../Alice-1-%d_1.png' % (i+1))			

img_num = len(img_list)
img_size = img_list[0].size
height = img_size[1]
width = img_size[0]

new_img = Image.new('RGBA',(width * img_num, height))
x = y = 0
cnt = 0

for img in img_list:
	new_img.paste(img, (x,y))
	cnt += 1
	x += width

new_img.show()
new_img.save('Alice-1.png', quality=100)
	
