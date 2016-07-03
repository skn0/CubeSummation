var Cube = {

    ProcessInput: function() {
        
        $("#resultPanel").remove();

        // setup some local variables
        var $form = $(this);

        // Let's select and cache all the fields
        //var $inputs = $form.find("input, select, button, textarea");
        var $input = $("#input");

        // Serialize the data in the form
        //var serializedData = $form.serialize();
        var serializedData = "value="+$input.val();

        // Let's disable the inputs for the duration of the Ajax request.
        // Note: we disable elements AFTER the form data has been serialized.
        // Disabled form elements will not be serialized.
        //$inputs.prop("disabled", true);
        $input.prop("disabled", true);

        // Fire off the request to /form.php
        request = $.ajax({
            url: "api/Cube",
            type: "post",
            data: serializedData
        });

        // Callback handler that will be called on success
        request.done(function (response, textStatus, jqXHR) {
            // Log a message to the console
            console.log("It worked!");
            var resultHtml = '<div id="resultPanel" class="panel panel-success"> <div class="panel-heading"> <h3 class="panel-title">Process Finished</h3> </div> <div class="panel-body"> ' + response + ' </div> </div>';
            $("#result").append(resultHtml);
            console.log("response: " + response);
            
        });
        
        // Callback handler that will be called on failure
        request.fail(function (jqXHR, textStatus, errorThrown) {
            
            // Log the error to the console
            console.error(
                "The following error occurred: " +
                textStatus, errorThrown
            );

            var info = 'An error occurred. Verify the information or contact the site administrator.'
            var resultHtml = '<div id="resultPanel" class="panel panel-danger"> <div class="panel-heading"> <h3 class="panel-title">Error</h3> </div> <div class="panel-body"> ' + info + ' </div> </div>';
            $("#result").append(resultHtml);
            
        });

        // Callback handler that will be called regardless
        // if the request failed or succeeded
        request.always(function () {
            // Reenable the inputs
            //$inputs.prop("disabled", false);
            $input.prop("disabled", false);
        });

        // Prevent default posting of form
        event.preventDefault();
        



    }

//End Cube namespace    
};

