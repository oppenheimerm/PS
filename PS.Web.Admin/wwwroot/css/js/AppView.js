export default class AppView {
    constructor(root, { onCloseMobileModal } = {}) {
        this.root = root;
        //this.onCloseMobileModal = onCloseMobileModal;

        window.addEventListener('scroll', function (e) {

            //  We have two instances of .header class(mobile / desktop),
            // so we can't just usd document.getElementById:
            const collection = document.getElementsByClassName("header");
            //  iterate th collection
            for (let i = 0; i < collection.length; i++) {

                if (this.window.scrollY >= 90) {
                    collection[i].classList.add('scroll-header');
                } else {
                    collection[i].classList.remove('scroll-header');
                }
            }
            //Only notify every 500 ms
        }, 500);
    }
}