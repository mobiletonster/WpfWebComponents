document.addEventListener('contextmenu', function(e) {
    if(!e.shiftKey){
        e.preventDefault();
        var menu = document.getElementById('context-menu');
        menu.style.left = e.pageX + 'px';
        menu.style.top = e.pageY + 'px';
        menu.classList.add("visible");
    } 
});

document.addEventListener('click', function() {
    var menu = document.getElementById('context-menu');
    menu.classList.remove("visible");
});
