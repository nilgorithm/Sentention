import os 
# r'C:\GUI\OpenNetFolder.exe\",1"'
# way_1 = r'@="\"C:\\Users\\@client_name\\Desktop\\VisualiseTableProd\\VisualiseTableProd.exe\",1"'
way_1 = r'@="\"C:\\Users\\KlychkovMD\\GUI\\Release\\net7.0-windows\\publish\\win-x64\\GUI.exe\",1"'

# way_1 = r'@="\"C:\\Program Files (x86)\\MooxAgent\\v0.9.29\\x64\\nw.exe\",1 \"--nwapp=C:\\Program Files (x86)\\MooxAgent\\v0.9.29\""'
# way_1 = r'@="\"C:\\Program Files (x86)\\MooxAgent\\v0.9.29\\x64\\nw.exe\",1"'


# way_2 = r'@="\"C:\\Users\\@client_name\\Desktop\\VisualiseTableProd\\VisualiseTableProd.exe\" \"%l\""'
way_2 = r'@="\"C:\\Users\\KlychkovMD\\GUI\\Release\\net7.0-windows\\publish\\win-x64\\GUI.exe\" \"%l\""'
# C:\Users\KlychkovMD\GUI\Release\net7.0-windows\publish\win-x64\
# way_2 = r'@="\"C:\\Program Files (x86)\\MooxAgent\\v0.9.29\\x64\\nw.exe\" \"%l\" \"--nwapp=C:\\Program Files (x86)\\MooxAgent\\v0.9.29\""'
# way_2 = r'@="\"C:\\Program Files (x86)\\MooxAgent\\v0.9.29\\x64\\nw.exe\" \"%l\""'
# \"--nwapp="\C:\\Program Files (x86)\\MooxAgent\\v0.9.29"\"
# \"--user-data-dir=%AppData%\\MooxAgent\"
s = 'Windows Registry Editor Version 5.00' + '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol]' + '\n'+'@="URL:MyProtocol"' + '\n' + '"URL Protocol"=""' + '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol\DefaultIcon]' + '\n' + way_1
s += '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol\shell]' + '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol\shell\open]' + '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol\shell\open\command]' + '\n' + way_2
# s = s.replace("@client_name", os.getlogin())

with open("r_file.reg",'w') as f:
    f.write(s)

os.system(r".\r_file.reg")


# import os 
# user = os.getlogin()
# # way_1 = rf'@="\"C:\\Users\\{user}\\GUI\\GUI.exe\",1"'
# way_1 = r'@="\"C:\\Users\\KlychkovMD\\GUI\\Release\\net7.0-windows\\publish\\win-x64\\VisualiseTableProd.exe\",1"'

# # way_2 = rf'@="\"C:\\Users\\{user}\\GUI\\GUI.exe\" \"%l\""'
# way_2 = r'@="\"C:\\Users\\KlychkovMD\\GUI\\Release\\net7.0-windows\\publish\\win-x64\\VisualiseTableProd.exe\" \"%l\""'

# s = 'Windows Registry Editor Version 5.00' + '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol]' + '\n'+'@="URL:MyProtocol"' + '\n' + '"URL Protocol"=""' + '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol\DefaultIcon]' + '\n' + way_1
# s += '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol\shell]' + '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol\shell\open]' + '\n' + '\n' + '[HKEY_CLASSES_ROOT\MyProtocol\shell\open\command]' + '\n' + way_2


# with open("r_file.reg",'w') as f:
#     f.write(s)

# os.system(r".\r_file.reg")