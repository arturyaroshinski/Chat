const messageBuilder = function () {
    let div = null;
    let header = null;
    let p = null;
    let footer = null;

    return {
        createMssage: function (classList) {
            div = document.createElement("div");
            if (classList === undefined)
                classList = [];

            classList.forEach(className => { div.classList.add(className) });
            div.classList.add("message");

            return this;
        },

        withHeader: function (text) {
            header = document.createElement("header");
            header.appendChild(document.createTextNode(text + " :"));
            return this;
        },

        withParagraph: function (text) {
            p = document.createElement("p");
            p.appendChild(document.createTextNode(text));
            return this;
        },

        withFooter: function (text) {
            footer = document.createElement("footer");
            footer.appendChild(document.createTextNode(text));
            return this;
        },

        build: function () {
            div.appendChild(header);
            div.appendChild(p);
            div.appendChild(footer);

            return div;
        }
    }
};