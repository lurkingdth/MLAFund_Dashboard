

var app = new Vue({

    el: "#app",
    data:{
        diselect:null,
        yes:false,
        tbl_Users: {
            id: null,
            name: null,
            username: null,
            password: null,
            district: null,
            remarks: null,
            user_role: null,
            dpt_name: null,
            dpt_code: null,
            status: null,
            securitystamp: null,
        },

        //Nishant Added code for Getting User data -start
        departmentList: null, //Added the department list array to push data from FillDepartment()
        //Nishant Added code for Getting User data -end
        

        districtList: null,
        busy: false,
    },
    mounted(){
        axios.all([
            axios.get("/api/common/District"),

        ]).then(axios.spread((data2,data1) => {          
            this.districtList = data2.data;
        }))
       .catch(error => alert(error))
       .finally(() => {                
       });
    },


    methods: {
        saveData(){
            this.$validator.validate().then(valid => {
                if (valid) {
                    if (this.busy == true) return;
                    this.busy = true;
                    this.busy = false;
                    this.tbl_Users.user_role = 5;
                    this.tbl_Users.dpt_code = 4;
                    var data = JSON.parse(JSON.stringify(this.tbl_Users))
                    console.log(data)
                    axios.post("/api/SaveNewUser/Save", data).then(d => {
                        if (d.data.status == "OK") {
                            alert("Data Sucessfully Saved")
                            window.location.href = "/Home/AddNewUser"
                        } else {
                            alert(d.data.Message)
                        }


                    })
                } else {
                    this.busy = false;
                    alert(data1.data.Message)
                }

            })
        },

        //Nishant Added code for Getting Department data -start
        // Comments: This function would call the '/api/Common/Department' route
        // and would populate the list of Departments from the [tbl_Users].
        // 
        FillDepartment() {
            axios.get("/api/common/DepartmentList").then(data => {
                this.departmentList = data.data;
            })
        },
        //Nishant Added code for Getting Department data -end

    }
})