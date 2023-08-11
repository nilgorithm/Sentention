function BuildButton() {
    let button = document.createElement("span");
    button.setAttribute('class', 'navTabButton');
    button.setAttribute('id', 'send-button');
    button.setAttribute('title', 'Show Dynamics CRM send button');


    let linkElement = document.createElement("a");
    linkElement.setAttribute("class", "navTabButtonLink");
    linkElement.setAttribute("title", "");

    let linkImageContainerElement = document.createElement("span");
    linkImageContainerElement.setAttribute("class", "navTabButtonImageContainer");

    let imageElement = document.createElement("img");
    let url_img = chrome.runtime.getURL("images/32_linear.png");
    imageElement.setAttribute("src", url_img);

    button.setAttribute('style', 'float:left; width:50px; height:50px;cursor:pointer!important');
    linkElement.setAttribute("style", "float:left; width:50px; height:50px;cursor:pointer!important;text-align:center");

    linkImageContainerElement.appendChild(imageElement);
    linkElement.appendChild(linkImageContainerElement);
    button.appendChild(linkElement);

    return button;
}

let button = BuildButton();

function initialize(obj){
    obj.prepend(button);

    button.onclick = function (){
        let frame = document.getElementsByTagName('iframe');
        for (let i = 0; i<frame.length; i++){
            if (frame[i].getAttribute("style").indexOf("visibility: visible") != -1){
                if(frame[i].contentDocument.body.innerHTML.indexOf('Тип задачи Произвести взыскание ДЗ – КЦ') != -1){
                    try{
                        let guid = frame[i].contentWindow.document.getElementById("crmFormSubmitId").value;
                        document.location.href = 'Myprotocol:' + document.getElementById("crmContentPanel").getAttribute("src")+":Form_2";
                    }
                    catch{
                        alert("quid не найден!");
                    }
                }
                else{
                    alert("Кнопка предназначена для вкладки 'Произвести взыскание!'");
                }
            }
        }
    }
}


// set up the mutation observer
var observer = new MutationObserver(function (mutations, me) {
  var obj = document.getElementById('navBar');
  if (obj) {
    initialize(obj);
    me.disconnect(); // stop observing
    return;
  }
});


// start observing
observer.observe(document, {
  childList: true,
  subtree: true
});
