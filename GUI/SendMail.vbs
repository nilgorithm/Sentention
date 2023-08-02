'application for compilation post form in outlook
msgTheme = Wscript.Arguments.Item(0)
msgBody = Wscript.Arguments.Item(1)
msgRecipe = Wscript.Arguments.Item(2)
msgAttach = Wscript.Arguments.Item(3)


Dim OutApp
Dim OutMail
Dim RecStr
Dim Body

splBody = Split(msgBody, "#")

allRecipe = Split(msgRecipe,",")

for each recipe in allRecipe
    RecStr = RecStr & recipe & ";"
next

'Wscript.echo Body

Set OutApp = CreateObject("Outlook.Application")
Set OutMail = OutApp.CreateItem(0)
              
On Error Resume Next

allAttach = Split(msgAttach,",")

With OutMail
    .To = RecStr
    .Subject = msgTheme
    .HTMLBody = msgBody
    for each attach in allAttach
        .Attachments.Add attach
    next
    .Display
End With

On Error GoTo 0
Set OutMail = Nothing
Set OutApp = Nothing