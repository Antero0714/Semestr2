document.addEventListener('DOMContentLoaded', () => {
    debugger; // Устанавливает точку останова
    const template = document.getElementById('cartoon-template').innerHTML;
    const container = document.getElementById('cartoons-container');

    fetch('/api/cartoons')
        .then(response => response.json())
        .then(data => {
            debugger; // Устанавливает точку останова
            data.forEach(item => {
                debugger;
                const block = template
                debugger;
                    .replace('{link}', item.Link)
                    .replace('{imagepath}', item.ImagePath)
                    .replace('{titlerus}', item.TitleRus)
                    .replace('{titleeng}', item.TitleEng)
                    .replace('{type}', item.Type)
                    .replace('{sound}', item.Sound);
                container.innerHTML += block;
            });
            debugger;
        })
    debugger;
        .catch(err => console.error('Ошибка загрузки данных:', err));
});