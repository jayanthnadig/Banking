(function(){  
    $(document).ready(function(e){
	
        $('#btnLogin').click(function(e){
            var user=$('#txtemail').val();
            var pass=$('#txtpass').val();
            Validation.LoginValidation();
            if(user!="" && pass!=""){
            loginService.login({"username":user,"password":pass});
        }
        else{
            alert("Invalid username");
        }
        });
        });
    })();