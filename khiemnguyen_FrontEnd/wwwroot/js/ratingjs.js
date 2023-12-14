
$('#mybtn').click(function (e) {
    var cment = $("#text").val();
    var id = $("#id").val();

    let i = 0;


    if ($("#star1").is(":checked")) {
        i = 1;
    }


    if ($("#star2").is(":checked")) {
        i = 2;
    }

    if ($("#star3").is(":checked")) {
        i = 3;
    }
    if ($("#star4").is(":checked")) {
        i = 4;
    }
    if ($("#star5").is(":checked")) {
        i = 5;
    }



    alert(i);



    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: 'SaveRate',
        data: { Id: id, star: i, comments: cment },
        success: function (Data) {
            alert(data.id);
            alert(data.name);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });



});
