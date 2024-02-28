const url = "https://localhost:7204/";
export function MakePostRequest(resource, body, callback){
    var xmlHttp = new XMLHttpRequest();
    
    xmlHttp.onreadystatechange = function() { 
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
            callback(xmlHttp.responseText);
        else if(xmlHttp.status == 400 || xmlHttp.status == 500)
            alert("An error occurred making the request. Please try again")
    }
    xmlHttp.open("POST", url + resource, true); // true for asynchronous 
    xmlHttp.setRequestHeader("Content-Type", "application/json")
    xmlHttp.send(body);
}

export function MakeGetRequest(resource, body, callback){
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function() { 
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
            callback(xmlHttp.responseText);
        else if(xmlHttp.status == 400 || xmlHttp.status == 500)
            alert("An error occurred making the request. Please try again")
    }
    xmlHttp.open("GET", url + resource, true); // true for asynchronous 
    xmlHttp.setRequestHeader("Content-Type", "application/json")
    xmlHttp.send(body);
}