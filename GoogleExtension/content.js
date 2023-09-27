function BuildButton() {
    let button = document.createElement("span");
    button.setAttribute('class', 'navTabButton');
    button.setAttribute('id', 'send-button');
    button.setAttribute('title', 'Create Message Button');


    let linkElement = document.createElement("a");
    linkElement.setAttribute("class", "navTabButtonLink");
    linkElement.setAttribute("title", "");

    let linkImageContainerElement = document.createElement("span");
    linkImageContainerElement.setAttribute("class", "navTabButtonImageContainer");
    linkImageContainerElement.setAttribute('style', 'margin-left: auto; margin-right: auto');

    let imageElement = document.createElement("img");
    let url_img = chrome.runtime.getURL("images/32_linear.png");
    imageElement.setAttribute("src", url_img);
    imageElement.setAttribute('style', 'margin-left: auto; margin-right: auto');

    button.setAttribute('style', 'float: left; width: 58px; height: 50px; cursor:pointer!important; margin-left: auto; margin-right: auto');
    linkElement.setAttribute('style', 'float: left; width: 36.99px; height: 50px; cursor:pointer!important; text-align:center; margin-left: auto; margin-right: auto;');

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
                if(frame[i].contentDocument.body.innerHTML.indexOf('Контрагент : Сведения') != -1){
                    try{
                        let guid = frame[i].contentWindow.document.getElementById("crmFormSubmitId").value;
                        console.log(guid)
                        document.location.href = 'Myprotocol:' + guid + ":Form_2";
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
  let obj = document.getElementById('navBar');

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
