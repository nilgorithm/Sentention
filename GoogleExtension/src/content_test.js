// "js": ["back.js","content.js"],
document.documentElement.style.height = '100%';
document.body.style.height = '100%';
document.documentElement.style.width = '100%';
document.body.style.width = '100%';
 
var div = document.createElement( 'div' );
var btnForm = document.createElement( 'form' );
var btn = document.createElement( 'input' );

//append all elements
document.body.appendChild( btn );
// document.body.appendChild( div );
// div.appendChild( btnForm );
// btnForm.appendChild( btn );
//set attributes for div
// div.id = 'myDivId';
//div.style.position = 'fixed';
// div.style.top = '50%';
// div.style.left = '50%';
// div.style.width = '25%';   
// div.style.height = '100%';
//div.style.backgroundColor = 'red';
 
//set attributes for btnForm
// btnForm.action = '';
 
//set attributes for btn
//"btn.removeAttribute( 'style' );
btn.type = 'button';
btn.value = '☎ -> Cyzer';
btn.style.position = 'absolute';
btn.style.top = '10%';
// btn.style.right = '0%';
// btn.style.left = '0%';


function getElementByXpath(path) {
    return document.evaluate(path, document.getElementById("contentIFrame0").contentWindow.document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.textContent;
  }
  
//   console.log( getElementByXpath("//html[1]/body[1]/div[1]") );
btn.onclick = function () {
//   alert(document.getElementById('Телефон_label'))
//   console.log(document.getElementsByClassName("js-vote-count flex--item d-flex fd-column ai-center fc-black-500 fs-title")[0])
//   console.log(document.getElementsByClassName("ms-crm-Field-Data-Print"))
//   console.log(document.getElementById('phonenumber_label'))
    // console.log(document.getElementsByClassName('ms-crm-FormSection'))
    // console.log(document.querySelector("#phonenumber > div.ms-crm-Inline-Value"))
    // console.log(getElementByXpath('//*[@id="Телефон_label"]/a'))
    // console.log(getElementByXpath('/html/body/div[2]/div/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div[1]/div/div[1]/div[1]/table/tbody/tr[5]/td[2]/div/div[1]/label/a'))
    // console.log(document.getElementById("contentIFrame0").contentWindow.document.getElementById("phonenumber"))
    console.log(document.getElementById("contentIFrame0").contentWindow.document.getElementById("phonenumber").getElementsByClassName("ms-crm-div-NotVisible ms-crm-div-NotSelectable")[0].textContent)
    console.log(getElementByXpath("/html/body/div[2]/div/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div[1]/div/div[1]/div[1]/table/tbody/tr[5]/td[2]/div/div[1]/label/a"))
    // console.log(document.getElementById("contentIFrame0").contentWindow.document.getElementById("phonenumber").document.getElementsByClassName("ms-crm-div-NotVisible ms-crm-div-NotSelectable"))
    // console.log(document.getElementById("contentIFrame0").contentWindow.document.getElementById("phonenumber"))
    // console.log(document.getElementById("contentIFrame0").contentWindow.document.getElementsByClassName("ms-crm-div-NotVisible ms-crm-div-NotSelectable"))
    // console.log(document.getElementById("contentIFrame0").contentWindow.document.getElementsById("Телефон_label"))
    // console.log(document.getElementsByClassName("ms-crm-CommandBar-Menu"))
    // console.log(document.getElementsByClassName("crmContentPanel").contentWindow.document.getElementById("contentIFrame0"))
    // console.log(getElementByXpath(document.getElementById('contentIFrame0').contentWindow.document.getElementById('ms-crm-FormSection')))
// let number = document.getElementById("number");
// number.addEventListener("click", async () => {
//   let [tab] = await chrome.tabs.query({ active: true, currentWindow: true });
//   chrome.scripting.executeScript({
//     target: { tabId: tab.id },
//     function: func,
//   });
// });
 
 
 
 
};
 
 
// document.documentElement.style.height = '100%';
// document.body.style.height = '100%';
// document.documentElement.style.width = '100%';
// document.body.style.width = '100%';
 
// let btn = document.createElement("button");
// btn.type = 'button';
// btn.value = 'hello';
// btn.style.position = 'absolute';
// btn.style.top = '50%';
// btn.style.left = '50%';
// // btn.innerHTML = "Save";
 
// };
// document.body.appendChild(btn);