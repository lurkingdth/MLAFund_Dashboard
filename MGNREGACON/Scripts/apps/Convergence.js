

var app = new Vue({

    el: "#app",
    data:{
        diselect:null,
        yes:false,
        convergence: {
            cid: null,
            district: null,
            block: null,
            panchayat: null,
            village: null,
            nameofscheme: null,
            fundofdpt: null,
            fundofmgnrega: null,
            totalfund: null,
            dateofsanctioned: null,
            timelimit: null,
            totalfundutilized: null,
            timespent: null,
            remarks: null,
            dateofentry: null,
            status: null,
            action: null,
            submittedby: null,
            convergence_doc: null,

            //convergencedpt: [],
            // Placed some default values in the array so that we can have one default Department section
            convergencedpt: [
                {
                    convergencedptname: null,
                    convergencedptscheme: null,
                    convergencefund: 0,
                }            ],
            
            
        },
        //compliancetypeList: null,
       // compliancetypedetailList:null,

        //Nishant Added code for Getting User data -start
        getUserDetails: null,
        departmentList: null, //Added the department list array to push data from FillDepartment()
        schemeList: null, //Added the scheme list array to push data from FillSchemes()
        userList: null, //Added the user list array to push data from FillUsername()
        //Nishant Added code for Getting User data -end
        

        districtList: null,
        blocklist:[],
        panchayatList:[],
        villageList:[],
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
        FillBlock(){
            axios.get('/api/Common/Block?district=' + this.convergence.district).then(data => {
                this.blocklist = data.data;
               
            })
        },
        FillPanchayat(){
            axios.get('/api/common/panchayat?district=' + this.convergence.district + '&block=' + this.convergence.block).then(data => {
                this.panchayatList=data.data;
            })
        },
        FillVillage(){
            axios.get('/api/common/village?district=' + this.convergence.district + '&block=' + this.convergence.block + '&panchayat=' + this.convergence.panchayat).then(data => {
                this.villageList=data.data;
            })

        },
        saveData(){
            this.$validator.validate().then(valid => {
                if (valid) {
                    if (this.busy == true) return;
                    this.busy = true;
                    var formData = new FormData();
                    var imagefile = document.querySelector('#filename_c');
                    this.getUserDetails = this.getUserDetailsMethod();
                   // var s1 = $("#someName").val();
                    formData.append("txtname", imagefile.files[0]);
                    axios.post('/Convergence/UploadFiles', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then(data1 => {
                        if (data1.data.status == "OK") {
                            this.convergence.convergence_doc = data1.data.file;
                            this.convergence.dateofentry = new Date();
                            this.convergence.status = "pending";
                            this.convergence.submittedby = this.getUserDetails;
                            this.busy = false;
                            var data = JSON.parse(JSON.stringify(this.convergence))
                            console.log(data)
                            axios.post('/api/Convergence/Save', data).then(d => {
                                if (d.data.status == "OK") {
                                    alert("Data Sucessfully Saved")
                                    window.location.href = '/Convergence/Create'
                                } else {
                                    alert(d.data.Message)
                                }


                            })
                        } else {
                            this.busy = false;
                            alert(data1.data.Message)
                        }
                    }).finally(_ => {

                    })
                }

            })
        },

        addDeptSector() {
            this.convergence.convergencedpt.push({
                //convergencedptid:null,
                //cid:null,
                convergencedptname: null,
                convergencedptscheme: null,
                convergencefund: 0,
            });
        },

        removeDeptSector(index) {
            this.convergence.convergencedpt.splice(index,1)
        },


        //Nishant Added code for Getting User data -start
        getUserDetailsMethod() {
            axios.get('/api/Common/UserDetails').then(data => {
                this.getUserDetails = data.data;
            })
        },
        //Nishant Added code for Getting User data -end

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


        //Nishant Added code for Getting Username for the corresponding departments - Start
        //Comments: This function would call the corresponding schemes as per
        //          the department selected
        FillSchemes() {
            //axios.get('/api/Common/Schemes?departmentName=' + this.convergencedptname).then(data => {
            //    this.schemeList = data.data;
            //})
            axios.get("/api/common/Schemes").then(data => {
                this.schemeList = data.data;
            })
        },
        //Nishant Added code for Getting Username for the corresponding departments - End

        //Nishant Added code for Getting available usernames from selected District and selected Department - Start
        FillUsername() {

            axios.get('/api/common/UsersList?district=' + this.convergence.district + '&dpt_name=' + this.convergence.convergencedpt).then(data => {
                this.userList = data.data;
            })

        },
        //Nishant Added code for Getting available usernames from selected District and selected Department - End

    }
})