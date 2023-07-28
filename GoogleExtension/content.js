
document.documentElement.style.height = '100%';
document.body.style.height = '100%';
document.documentElement.style.width = '100%';
document.body.style.width = '100%';

document.addEventListener('readystatechange', () => console.log(document.readyState));

function bt(){
    var btn = document.createElement( 'input' );
    btn.id = 'b-1'
    btn.type = 'button';
    btn.value = 'История звонков';
    btn.style.color = 'white';
    // btn.style.position = 'absolute';
    // btn.style.top = '10%';
    return btn
}

function bt_3(){
    var btn_3 = document.createElement( 'input' );
    btn_3.id = 'b-3'
    btn_3.type = 'button';
    btn_3.value = 'Отправить Уведомление';
    btn_3.style.color = 'white';
    return btn_3
}

var btn = bt() 
var btn_3 = bt_3() 

// let checkPath = "/html/body/div[2]/div/div[2]/div/div[1]/div/div/div[1]/div[1]/div/div[2]/div/div[1]/span/div[2]/h1"
let phoneNumberPath = "/html/body/div[2]/div/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div[1]/div/div[1]/div[1]/table/tbody/tr[5]/td[2]/div/div[1]/label/a"

function getElementByXpath(path) {
    return document.evaluate(path, document.getElementById("contentIFrame0").contentWindow.document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.textContent;
}

function arr_contains(arr, pred) {
    for (var i = 0; i < arr.length; i++) {
        if (typeof pred == 'function' && pred(arr[i], i, arr) || arr[i] === elem) {
            return true;
        }
    }
    return false;
}

var container = []
document.addEventListener("DOMSubtreeModified", function(event){
    if(document.getElementById("phonecall|NoRelationship|Form|Mscrm.Form.phonecall.Close")) 
    // if(getElementByXpath(checkPath) == 'Создать объект Звонок')
    {   
        if(document.body.contains(btn))
        {
            console.log("__exists_btn__")
            if(!document.body.contains(btn_3))
            {
                console.log("__deleted_btn_3__")
            }
            else
            {
                document.body.removeChild(btn_3)
            }
        }
        else 
        {   
            document.body.appendChild( btn );
            //document.body.appendChild( btn_2 );
            //document.body.removeChild( btn_3 );
            //document.body.appendChild( btn_2 );
            btn.onclick = function () 
            {   
                try
                {   
                    //var el = getElementByXpath(phoneNumberPath)
                    // console.log(Xrm.Page.data.entity.getId())
                    var el = document.getElementById("contentIFrame0").contentWindow.document.getElementById("phonenumber").getElementsByClassName("ms-crm-div-NotVisible ms-crm-div-NotSelectable")[0].textContent
                    if(arr_contains(container,el))
                    {
                        console.log(arr_contains(container,el))
                        window.onbeforeunload = function() {
                            return "Требуется перезагрузить страницу и повторить попытку. Перезагрузить?";
                          };
                        document.location.reload(true)
                    }
                    else
                    {
                        //container.push(el)
                        //console.log(container)
                        document.location.href = 'Myprotocol:'+el+":Form_1"
                    }
                    
                }
                catch
                {   
                    console.log("Error_1")
                    window.onbeforeunload = function() {
                        return "Требуется перезагрузить страницу и повторить попытку. Перезагрузить?";
                      };
                    document.location.reload(true)
                }
            }
        }
    }
    else if(document.getElementById("task|NoRelationship|Form|gpbl.task.Collection.TaskForm.Button"))
    {
        if(document.body.contains(btn_3))
        {
            console.log("__exists_btn3__")
            if(!document.body.contains(btn))
            {
                console.log("__deleted_btn__")
            }
            else
            {
                document.body.removeChild()
            }
        }
        else
        {
            document.body.appendChild( btn_3 );
            try
            {
                btn_3.onclick = function (){
                    document.location.href = 'Myprotocol:'+document.getElementById("crmContentPanel").getAttribute("src")+":Form_2"
                }
            }
            catch
            {
                document.location.reload(true)     
            }
        }
    }
    
});



// //наблюдение за изменениями:
// var mutationObserver = new MutationObserver(function(mutations) {
//     mutations.forEach(function(mutation) {
//       console.log(mutation);
//     });
// });

// // Запускаем наблюдение за изменениями в корневом HTML-элементе страницы
// mutationObserver.observe(document.documentElement, {
//     attributes: true,
//     characterData: true,
//     childList: true,
//     subtree: true,
//     attributeOldValue: true,
//     characterDataOldValue: true
// });