

var app = new Vue({

    el: "#app",
    data:{
        diselect:null,
        yes:false,
        workprogress: {
            pid: null,
            cid: null,
            fundutilizeddept: null,
            fundutilizedmgnrega: null,
            fundutilized: null,
            geotagphoto: null,
            workstatus: null,
            mandaysgenerated: null,
            remarks: null,
            dateofutilization: null,
            submittedby: null,
            date: null,
        },
        //compliancetypeList: null,
       // compliancetypedetailList:null,

        //Nishant Added code for Getting User data -start
        getUserDetails: null,
        //Nishant Added code for Getting User data -end
        
        busy: false,
    },

    methods: {
        saveData(){
            this.$validator.validate().then(valid => {
                if (valid) {
                    if (this.busy == true) return;
                    this.busy = true;
                    var formData = new FormData();
                    var imagefile = document.querySelector('#filename_c');
                    this.getUserDetails = this.getUserDetailsMethod();
                    formData.append("txtname", imagefile.files[0]);
                    axios.post('/Convergence/UploadFiles', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then(data1 => {
                        if (data1.data.status == "OK") {
                            this.workprogress.geotagphoto = data1.data.file;
                            this.workprogress.submittedby = this.getUserDetails;
                            this.workprogress.date = new Date();
                            this.busy = false;
                            var data = JSON.parse(JSON.stringify(this.workprogress))
                            console.log(data)
                            axios.post('/api/WorkProgress/Save', data).then(d => {
                                if (d.data.status == "OK") {
                                    alert("Data Sucessfully Saved")
                                    window.location.href = '/Convergence/Workprogress'
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

        //Nishant Added code for Getting User data -start
        getUserDetailsMethod() {
            axios.get('/api/Common/UserDetails').then(data => {
                this.getUserDetails = data.data;
            })
        },
        //Nishant Added code for Getting User data -end

        
    }
})