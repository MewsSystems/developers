export default (text) => {
    const id = 'hidden-clipboard-textarea';
    let existsTextarea = document.getElementById(id);

    if (!existsTextarea) {
        const textarea = document.createElement('textarea');
        textarea.id = id;
        textarea.style.position = 'fixed';
        textarea.style.top = 0;
        textarea.style.left = 0;
        textarea.style.width = '1px';
        textarea.style.height = '1px';
        textarea.style.padding = 0;
        textarea.style.border = 'none';
        textarea.style.outline = 'none';
        textarea.style.boxShadow = 'none';
        textarea.style.background = 'transparent';
        document.querySelector('body').appendChild(textarea);
        existsTextarea = document.getElementById(id);
    }

    existsTextarea.value = text;
    existsTextarea.select();
    document.execCommand('copy');
};
