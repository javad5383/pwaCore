
$(function () {
    // Multiple images preview in browser
    var imagesPreview = function (input, placeToInsertImagePreview) {

        if (input.files) {
            var filesAmount = input.files.length;

            for (i = 0; i < filesAmount; i++) {
                var reader = new FileReader();

                reader.onload = function (event) {
                    $($.parseHTML('<img class="selectedimg2" >')).attr('src', event.target.result).appendTo(placeToInsertImagePreview);
                }

                reader.readAsDataURL(input.files[i]);
            }
        }

    };

    $('#gallery-photo-add').on('change', function () {

        imagesPreview(this, '#gallery');
    });

    //change image after click in single shop page
    $(".thmb").click(function () {
        var mainImg = $(".imgth").attr('src');
        $(".imgth").attr('src', `${this.src}`);
        this.src = mainImg;
    });

});




