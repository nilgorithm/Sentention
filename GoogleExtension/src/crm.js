
document.addEventListener("DOMContentLoaded", function (event) {
debugger;
            var settings = new Gpbl.Settings(parent.Xrm.Page.context),
                baseUrl = settings.get('Reporting.Url'),
                urlData = window.location.href.split("=");
            if (urlData[1]) {
                //var url = "http://dtc01-sql01.corp.gpbl.ru/ReportServer_CRM?/" + decodeURIComponent(urlData[1]);
                var url = baseUrl + "?/" + decodeURIComponent(urlData[1]);
                var iframe = document.getElementById("rep");
                iframe.style.height = url.split("$")[1] || "230px";
                iframe.src = url.split("$")[0] || url;

                // Хендлер рефреша фрейма
                window.onmessage = function (event) {
                    setTimeout(function () {
                        iframe.src = iframe.src;
                    }, 5000);
                };
            }
        });