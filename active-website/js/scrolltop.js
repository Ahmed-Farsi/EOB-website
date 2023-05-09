let calcScrollValue = () => {
    let scrollProgress = document.getElementById
    ("progress");
    let progressValue = document.getElementById
    ("progress-value");
    let pos = document.documentElement.scrollTop;
    let calcHeight =
        document.documentElement.scrollHeight - 
        document.documentElement.clientHeight;
    let scrollValue = Math.round((pos * 100) / calcHeight);

    if(pos>100){
        scrollProgress.style.display = "grid";
    }
    else{
        scrollProgress.style.display = "none";
    }

    scrollProgress.addEventListener("click", () => {
        document.documentElement.scrollTop = 0;

    scrollProgress.style.background = 'background: linear-gradient(35deg,rgb(6,0,151) 0% ${scrollValue}%, rgb(130,4,255) 73% ${scrollValue}%, rgb(193,15,255) 100% ${scrollValue}%);'
    })

};

window.onscroll = calcScrollValue;
window.onload = calcScrollValue;