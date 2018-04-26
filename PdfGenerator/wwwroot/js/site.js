// Write your JavaScript code.
(function ($) {
    function HomeIndex() {
        var $this = this;

        function initialize() {
            $('#Content').summernote({
                focus: true,
                height: 450,
                codemirror: {
                    theme: 'united'
                }
            });

            $('.marginTop30').css("margin-top", "30px");

            $('#btnSubmit').click(function (e) {
                e.preventDefault();
                

                var title = $('#Title').val();
                var content = $('#Content').val();

                if (title == '' || title == null || content == '' || content == null) {
                    $('#fillAll').show();
                    setTimeout(function () {
                        $('#fillAll').hide();
                    }, 3000);
                } else {
                    $('#PDFalert').show();
                    return fetch("/Home/GeneratePdf", {
                        method: 'POST',
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(content)
                    }).then(response => response.blob({ type: 'application/pdf' }))
                        .then(blob => URL.createObjectURL(blob))
                        .then(url => {
                            var link = document.createElement('a');
                            link.href = url;
                            link.download = title + ".pdf";
                            link.click();
                            setTimeout(function () {
                                $('#PDFalert').hide();
                            }, 2000);
                            console.log('open');
                        })
                        .catch(function (error) {
                            throw new Error(error);
                        });
                }

                

                
            });
        }

        $this.init = function () {
            initialize();
        }
    }
    $(function () {
        var self = new HomeIndex();
        self.init();
    })
}(jQuery))
