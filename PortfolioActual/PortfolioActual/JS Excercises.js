//Begin 5 numbers excercise//
$("#productbtn").click(function product() {
    //parse user input into usable numbers
    var num1 = parseInt($("#integer1").val(), 10);
    var num2 = parseInt($("#integer2").val(), 10);
    var num3 = parseInt($("#integer3").val(), 10);
    var num4 = parseInt($("#integer4").val(), 10);
    var num5 = parseInt($("#integer5").val(), 10);
    //checks for valid user input
    if (isNaN(num1) || isNaN(num2) || isNaN(num3) || isNaN(num4) || isNaN(num5)) {
        alert("Please enter a real number in each box")
    } else {
        var lowest = Math.min(num1, num2, num3, num4, num5);
        var highest = Math.max(num1, num2, num3, num4, num5);
        $("#min").html("The lowest of the numbers is: " + lowest);
        $("#max").html("The highest of the numbers is: " + highest);
        $("#sum").html("The sum of the numbers is: " + (num1 + num2 + num3 + num4 + num5));
        $("#mean").html("The mean of the numbers is: " + (num1 + num2 + num3 + num4 + num5) / 5);
        $("#product").html("The product of the numbers is: " + (num1 * num2 * num3 * num4 * num5));
    };
});

//clears fields and outputs on modal close
$("#closebtn1").click(function () {
    $("#min").html("");
    $("#max").html("");
    $("#sum").html("");
    $("#mean").html("");
    $("#product").html("");
    $("#integer1").val(0);
    $("#integer2").val(0);
    $("#integer3").val(0);
    $("#integer4").val(0);
    $("#integer5").val(0);
    //$("#5numshighlighter").class(brush: js text; collapse: true);
});
//End 5 numbers excercise//

//Start Factorials//
$("#factorbtn").click(function () {
    var x = parseInt($("#userFactor").val(), 10);
    function factorial(x) {
        if (isNaN(x) || x < 0) {
            $("#error").html("Please enter a non-negative integer!");
            return;
        }

        if (x === 0) {
            return 1;
        } 
            return x * factorial(x - 1);
    }
    $("#correct").html("The factorial of your number is: " + factorial(x));
});

$("#closebtn2").click(function () {
    $("#correct").html("");
    $("#error").html("");
    $("#userFactor").val(0);
});
//End Factorials//

//Start FizzBuzz//
$("#fizzbtn").click(function fizzbuzz() {
    var val1 = parseInt($("#fizzinput1").val(), 10);
    var val2 = parseInt($("#fizzinput2").val(), 10);

    //Checks user input to ensure operable numbers (i.e. no letters, negatives or outside range)//
    if (isNaN(val1) || val1 < 1 || val1 > 100 || isNaN(val2) || val2 < 1 || val2 > 100 || val1 === val2) {
        $("#fizzerror").html("Please enter 2 differing, valid numbers between 1 and 100 in each box.");
        $("#fizzinput1").val(0);
        $("#fizzinput2").val(0);
        return;
    } else {
        $("#fizzerror").html("");
    };

    var output = '';
    for (var i=1;i<=100;i++){
        if (i%val1 === 0 && i%val2 === 0){
            output = output + "FizzBuzz | ";
        }
        else if (i % val2 === 0) {
            output = output + "Buzz | ";
        }
        else if (i % val1 === 0) {
            output = output + "Fizz | ";
        }
        else {
            output = output + i + " | ";
        }
    }
    return $("#fizzcorrect").text(output);
});

$("#closebtn3").click(function () {
    $("#fizzerror1").html("");
    $("#fizzerror2").html("");
    $("#fizzcorrect").html("");
    $("#fizzinput1").val(0);
    $("#fizzinput2").val(0);
});
//End FizzBuzz//

//Start Palindromes//
$("#palbtn").click(function palindrome() {
    var pal1 = $("#palinput").val();
    var palnospace = pal1.replace(/ /g,"")
    var palsmall = palnospace.toLowerCase();
    var palarray = palsmall.split("")  
    var palactual = palarray.reverse();
    var paloutput = palactual.join("");
    if (palsmall === paloutput) {
        $("#paltrue").html("This word IS a palindrome!");
    } else {
        $("#paltrue").html("This word is NOT a palindrome.")
    };
});


$("#closebtn4").click( function () {
    $("#paltrue").html("");
    $("#palinput").val("");
});
//End Palindromes//