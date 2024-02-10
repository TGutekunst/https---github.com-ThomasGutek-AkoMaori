let userCache = {
login: false,
currentUser: "",
currentPassword: ""
};

/* Tab menu */
function openTab(tabName) {
    // Declare all variables
    let i, tabcontent;
  
    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
      tabcontent[i].style.display = "none";
    }
  
   // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(tabName).style.display = "block";
  }


  /* Display version */
  const displayVersion = () => {
    const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/EventCount",
      {
          headers: {
              "Accept": "text/plain",
          },
        }
      );
    const streamPromise = fetchPromise.then((response) => response.text());
    streamPromise.then((data) => document.getElementById("version").textContent = "Version " + data);
   }
   displayVersion();

  /* Dynamically create events table based on current events */

  const getEventCount = () => {
    const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/EventCount",
      {
          headers: {
              "Accept": "text/plain",
          },
        }
      );
    const streamPromise = fetchPromise.then((response) => response.text());
    streamPromise.then((data) => makeEventsTable(data));
   }

   const makeEventsTable = (eventCount) => {
    let htmlString = "";
    let eventDataArr = [];

    const addEvent = (event, i) => {
    
    const lines = event.split('\n');
    const eventData = [];

    for (const line of lines) {
      const [key, value] = line.split(':');
      const trimmedKey = key.trim();
      const trimmedValue = value.trim();
      eventData[trimmedKey] = trimmedValue;
      eventDataArr.push(eventData);
    }
      htmlString += `<tr><td class='itemTitle'>${convertDTSTARTToEnglish(eventData.DTSTART, 1)}</td><td> <span class='itemTitle'>${eventData.SUMMARY}: </span>
        <span class='text'>${eventData.DESCRIPTION}<br>
        Starts: ${convertDTSTARTToEnglish(eventData.DTSTART, 0)}<br> Finishes: ${convertDTSTARTToEnglish(eventData.DTEND, 0)}</span></td>
        <td><a href='https://cws.auckland.ac.nz/ako/api/Event/${i}'> Add To Calendar </a></td></tr>`;
    // Make Table
    const table = document.getElementById("EventsTable")
    table.innerHTML = htmlString;
    //console.log(htmlString);
    // Add event button
    //console.log(eventDataArr[i]);
  }
      

    for(let i = 0; i < eventCount; i++)
    {
      const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/Event/" + i,
      {
          headers: {
              "Accept": "text/calender",
          },
        }
      );
    const streamPromise = fetchPromise.then((response) => response.text());
    streamPromise.then((data) => addEvent(data, i));
   }
  }

  function convertDTSTARTToEnglish(dtStart, header) {
    // Parse the iCalendar date string
    const year = parseInt(dtStart.substr(0, 4));
    const month = parseInt(dtStart.substr(4, 2)) - 1; // Months are zero-based (0-11)
    const day = parseInt(dtStart.substr(6, 2));
    const hour = parseInt(dtStart.substr(9, 2));
    const minute = parseInt(dtStart.substr(11, 2));
    const second = parseInt(dtStart.substr(13, 2));
  
    // Create a Date object with the parsed values
    const date = new Date(Date.UTC(year, month, day, hour, minute, second));
  
    // Format the date as a standard English string
    const detailsFormat = { weekday: 'long', day: 'numeric', month: 'long',  year: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric'};
    const headerFormat = { day: 'numeric', month: 'long',  year: 'numeric'};
    
    if (header == 1)
      {
        return date.toLocaleDateString('en-US', headerFormat);
      }
    return date.toLocaleDateString('en-US', detailsFormat);
  }
  


  /* Dynamically make shop table */

  const getItems = () => {
    const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/AllItems",
      {
          headers: {
              "Accept": "application/json",
          },
        }
      );
    const streamPromise = fetchPromise.then((response) => response.json());
    streamPromise.then((data) => makeItemsTable(data));
  }


   const makeItemsTable = (items) => {
    let htmlString = "";

    const addItem = (item) => {
      // Get item image and details
      htmlString += `<tr><td><img class='itemImage' src=https://cws.auckland.ac.nz/ako/api/ItemImage/${item.id}></td><td> <span class='itemTitle'>${item.name}</span><span class='text'><br>${item.description}<br>${item.price}<br></span><button id='buyButton' onclick='purchase(${item.id})'>Buy Now!</button><div></div></td></tr>`;
    }
    items.forEach(addItem);

    const table = document.getElementById("ItemsTable")
    table.innerHTML = htmlString;
  }

  /* Dynamically update table */

  const getNewItems = () => {
    let term = document.getElementById("searchBar").value;
    if(term == '') getItems();
    console.log(term);
    const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/Items/" + term,
      {
          headers: {
              "Accept": "application/json",
          },
        }
      );
    const streamPromise = fetchPromise.then((response) => response.json());
    streamPromise.then((data) => makeItemsTable(data));
  }

  /* Buy now button */
  const purchase = (id) => {
    if (userCache.login == true){
    const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/PurchaseItem/" + id,
      {
          headers: {
              "Accept": "text/plain",
              Authorization : 'Basic ' + btoa(userCache.currentUser + ':' + userCache.currentPassword)
          },
        }
      );
    const streamPromise = fetchPromise.then((response) => response.json());
    streamPromise.then((data) =>
    {
      console.log(data);
      alert("Thank you " + data.userName + " for buying " + data.productID);

    });
    }
    else
    openTab('UserLogin');
  }



  /* Dynamicaly make learning game table */
  const leftMatch = [];
  const rightMatch = [];
  let score = 0;
  let pairsLength = 0;
  const getPairs = () => {
    score = 0;
    const scoreTextReset = document.getElementById("ScoreText");
    scoreTextReset.textContent =  ``;
    const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/MatchingPair",
      {
          headers: {
              "Accept": "application/json",
          },
        }
      );
    const streamPromise = fetchPromise.then((response) => response.json());
    streamPromise.then((data) => makeGamesTable(data));
  }


   const makeGamesTable = (pairs) => {
    let htmlString = "";
    //split pairs into array of matching pairs
    splitPairs = pairs.pairs.split('|')
    pairsLength = splitPairs.length;
    // const scoreText = document.getElementById("ScoreText");
    // scoreText.textContent =  `Your current score is ${score} out of ${pairsLength}`;
    //console.log(splitPairs);
    //console.log(pairs.type);

    const addPair = (type, i) => {
      // CHANGE THESE
      let unrandomisedLeftIndex = 0;
      let unrandomisedRightIndex = 0;
      let j = 0;
    

      //find where random and matching meet up
      leftMatch.forEach((left)=>{
        if (leftRand[i] == left) unrandomisedLeftIndex = j;
        j++;
      });
      j = 0;
      rightMatch.forEach((right)=>{
        if (rightRand[i] == right) unrandomisedRightIndex = j;
        j++;
      });
      let typeSplit = type.split(':');
      let leftType = typeSplit[0];
      let rightType = typeSplit[1];
      switch (leftType){
        case "string": htmlString += `<tr><td class='gameCell'>${leftRand[i]}</td><td  class='gameCell' id="s${unrandomisedLeftIndex}" ondrop='drop(event)' ondragover='dragOver(event)'></td>`;
        break;
        case "string": htmlString += `<tr class='gameRow'><td>${leftRand[i]}</td><td id="s${unrandomisedLeftIndex}" ondrop='drop(event))' ondragover='dragOver(event)'></td>`;
        break;
        case "image":  htmlString += `<tr class='gameRow'><td><img class='gameImg' src='${leftRand[i]}'></td><td  id="s${unrandomisedLeftIndex}" ondrop='drop(event))' ondragover='dragOver(event)'></td>`;
        break;
        case "string":  htmlString += `<tr class='gameRow'><td>${leftRand[i]}></td><td  id="s${unrandomisedLeftIndex}" ondrop='drop(event))' ondragover='dragOver(event)'></td>`;
        break;
      }
      switch (rightType){
        case "string": htmlString += `<td class='gameCell'> <span draggable='true' ondragstart='dragStart(event)' id="t${unrandomisedRightIndex}">${rightRand[i]}</span></td></tr>`;
        break;
        case "image": htmlString += `<td><img class='gameImg'  draggable='true' ondragstart='dragStart(event)  id="t${unrandomisedRightIndex}"' src='${rightRand[i]}'></td></tr>`;
        break;
        case "string":  htmlString += `<td> <span draggable='true' ondragstart='dragStart(event)'  id="t${unrandomisedRightIndex}">${rightRand[i]}</span></td></tr>`;
        break;
        case "audio":  htmlString += `><td><audio controls src ='${rightRand[i]}' draggable='true' ondragstart='dragStart(event)'  id="t${unrandomisedRightIndex}"></td></tr>`;
        break;
      }

      
    }

    const matchSplit = (splitPairs) => {
        //split match into array of question and answer
        match = splitPairs.split('@');
        leftMatch.push(match[0]);
        rightMatch.push(match[1]);
    }

    function shuffleSide (side) {
      for (let i = side.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [side[i], side[j]] = [side[j], side[i]];
      }
      return side;
    }

    let leftRand = shuffleSide(leftMatch);
    let rightRand = shuffleSide(rightMatch);
    //console.log(leftRand);
    splitPairs.forEach(matchSplit);
    let i = 0;
    splitPairs.forEach(() => {
      addPair(pairs.type, i);
      i++
    })

    const table = document.getElementById("GameTable");
    //console.log(htmlString);
    table.innerHTML = htmlString;
  }

  /* Handle drops */

  const check =() =>{
    const scoreText = document.getElementById("ScoreText");
    scoreText.textContent =  `Your current score is ${score} out of ${pairsLength}`;
  }

  const dragStart = (ev) => {
    ev.dataTransfer.setData("text/plain", ev.target.id);
  }

  const dragOver = (ev) => {
    ev.preventDefault();
  }

  const drop = (ev) => {

    if(ev.dataTransfer !== null) {
      const sourceData = ev.dataTransfer.getData("text/plain");
      const target = ev.target;
      target.appendChild(document.getElementById(sourceData));
      console.log(sourceData.substring(1) + ' ' + target.id.substring(1));
      if(sourceData.substring(1) == target.id.substring(1)){
        score++;
        console.log(`Your current score is ${score} out of ${pairsLength}`);
        //const scoreText = document.getElementById("ScoreText");
        //scoreText.textContext = `Your current score is ${score} out of ${pairsLength}`
        sourceData.draggable = false;
      }
    }


  }


  /* Register a new user */
  function registerUser()
  {
     const userData = {
       username: document.getElementById("UsernameReg").value,
       password: document.getElementById("PasswordReg").value,
       address: document.getElementById("AddressReg").value
     }
     const regJson = JSON.stringify(userData);
 
     const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/Register",
     {
       headers : {
               "Content-Type" : "application/json",
             },
             body : regJson,
             method : "POST",
       }
     );
   const streamPromise = fetchPromise.then((response) => response.text());
   streamPromise.then((data) => {
    //alert(data);
     const userReg = document.getElementById("UsernameReg");
     const passReg = document.getElementById("PasswordReg");
     const addrReg = document.getElementById("AddressReg");
     const succReg = document.getElementById("SuccessReg");
     userReg.value = "";
     passReg.value = "";
     addrReg.value = "";
     succReg.textContent = data;
      });
 }
  /* Log in a user */

  function userLogin()
  {
    const username = document.getElementById("Username").value;
    const password = document.getElementById("Password").value;
    //console.log(username + password);
    const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/TestAuth",
    {
      headers : {
        Authorization : 'Basic ' + btoa(username + ':' + password),
          },
      }
    );

    const streamPromise = fetchPromise.then((response) =>
    {
      console.log(response);
      if (response.ok)
      {
        userCache.login = true;
        userCache.currentUser = username;
        userCache.currentPassword = password;  
        const loginTab = document.getElementById("LoginTab");
        const logout = document.getElementById("CurrentUser");
        loginTab.style.display = 'none';
        logout.textContent = (username + " (Logout)");
        openTab('Home');
      }
      else
      {
        const fail = document.getElementById("failLog");
        fail.textContent = "Incorrect Credentials. Try again";
      }

       });

  }

  /* Log out user */
  const logOut = () => {
    userCache.login = false;
    userCache.currentUser = "";
    userCache.currentPassword = ""; 
    const loginTab = document.getElementById("LoginTab");
    loginTab.style.display = 'inline';
    const logout = document.getElementById("CurrentUser");
    logout.textContent = ("");
  }

  /* Refresh input fields */
  const refreshLog = () => {
  const uinput = document.getElementById("Username");
  const pinput = document.getElementById("Password");
  uinput.value = "";
  pinput.value = "";
  }
   /* Upload a comment to api */
 function uploadComment()
   {
    const commentData = {
      comment: document.getElementById("Comment").value,
      name: document.getElementById("Name").value
    }
    const commentJson = JSON.stringify(commentData);

    const fetchPromise = fetch("https://cws.auckland.ac.nz/ako/api/Comment",
    {
      headers : {
              "Content-Type" : "application/json",
            },
            body : commentJson,
            method : "POST",
      }
    );
  const streamPromise = fetchPromise.then((response) => response.text());
  streamPromise.then((data) => {
    const myIframe = document.getElementById("RecentEntries");
    const commentBox = document.getElementById("Comment");
    const nameBox = document.getElementById("Name");
    commentBox.value = "";
    nameBox.value = "";
    myIframe.src = myIframe.src;});
}
