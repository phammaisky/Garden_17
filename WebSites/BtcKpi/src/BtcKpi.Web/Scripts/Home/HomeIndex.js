$(function () {
    //IPF Years
    var bar = new Morris.Bar({
        element: 'bar-chart',
        resize: true,
        data: [
            { y: 'T1', a: 90, b: 10 },
            { y: 'T2', a: 75, b: 25 },
            { y: 'T3', a: 50, b: 50 },
            { y: 'T4', a: 75, b: 25 },
            { y: 'T5', a: 50, b: 50 },
            { y: 'T6', a: 75, b: 25 },
            { y: 'T7', a: 80, b: 20 },
            { y: 'T8', a: 85, b: 15 },
            { y: 'T9', a: 90, b: 10 },
            { y: 'T10', a: 78, b: 22 },
            { y: 'T11', a: 86, b: 14 },
            { y: 'T12', a: 99, b: 1 }
        ],
        barColors: ['#00a65a', '#f56954'],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Đạt', 'Không đạt'],
        hideHover: 'auto'
    });
    

    //// UPF Cross
    //var line = new Morris.Line({
    //    element: 'line-chart',
    //    resize: true,
    //    data: [
    //      { y: '2020 Q1', item1: 100 },
    //      { y: '2020 Q2', item1: 150 },
    //      { y: '2020 Q3', item1: 170 },
    //      { y: '2020 Q4', item1: 160 },
    //      { y: '2021 Q1', item1: 180 },
    //      { y: '2021 Q2', item1: 190 },
    //      { y: '2021 Q3', item1: 185 },
    //      { y: '2021 Q4', item1: 175 },
    //      { y: '2023 Q1', item1: 190 },
    //      { y: '2023 Q2', item1: 195 }
    //    ],
    //    xkey: 'y',
    //    ykeys: ['item1'],
    //    labels: ['Item 1'],
    //    lineColors: ['#3c8dbc'],
    //    hideHover: 'auto'
    //});

    // AREA CHART
    var area = new Morris.Area({
        element: 'revenue-chart',
        resize: true,
        data: [
            { y: '2020 Q1', item1: 2666, item2: 2666 },
            { y: '2020 Q2', item1: 2778, item2: 2294 },
            { y: '2020 Q3', item1: 4912, item2: 1969 },
            { y: '2020 Q4', item1: 3767, item2: 3597 },
            { y: '2021 Q1', item1: 6810, item2: 1914 },
            { y: '2021 Q2', item1: 5670, item2: 4293 },
            { y: '2021 Q3', item1: 4820, item2: 3795 },
            { y: '2021 Q4', item1: 15073, item2: 5967 },
            { y: '2022 Q1', item1: 10687, item2: 4460 },
            { y: '2022 Q2', item1: 8432, item2: 5713 }
        ],
        xkey: 'y',
        ykeys: ['item1', 'item2'],
        labels: ['Item 1', 'Item 2'],
        lineColors: ['#a0d0e0', '#3c8dbc'],
        hideHover: 'auto'
    });

    //Xếp loại IPF năm
    var donut = new Morris.Donut({
        element: 'sales-chart',
        resize: true,
        colors: ["#3c8dbc", "#f56954", "#00a65a"],
        data: [
            { label: "Đạt", value: 70 },
            { label: "Không đạt", value: 25 },
            { label: "Loại A", value: 5 }
        ],
        hideHover: 'auto'
    });

    //DONUT CHART
    var donut2 = new Morris.Donut({
        element: 'upf-chart',
        resize: false,
        colors: ["#3c8dbc", "#f56954", "#00a65a"],
        data: [
            { label: "Đạt", value: 95 },
            { label: "Không đạt", value: 5 },
            { label: "Loại A", value: 10 }
        ],
        hideHover: 'auto'
    });
    
});