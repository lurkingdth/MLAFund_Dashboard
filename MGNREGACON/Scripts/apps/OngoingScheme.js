

var app = new Vue({

    el: "#app",
    data:{
        diselect:null,
        yes:false,
        completeenrty: {
            ceid: null,

division                           :null,
subdivision                        :null,
district                           :null,
block                              :null,
panchayat                          :null,
village                            :null,
recommendedby                      :null,
mlampname                          :null,
nameofconsti                       :null,
sanction_year                      :null,
schemename                         :null,
typeofscheme                       :null,
technologyused                     :null,
type                               :null,
roadbridgelenghth                  :null,
completedlengthtillnow             :null,
balancelengthtillnow               :null,
administrativesanctionamount       :null,
referenceofadministrativeapproval  :null,
dateofadministrativeapproval       :null,
agreementamount                    :null,
totexpenditureprevious             :null,
totexpenditurecurrent              :null,
contractorname                     :null,
regidcontractor                    :null,
dateofagreement                    :null,
agreementnumber                    :null,
dateofcompletionasperagreement     :null,
technicalamount                    :null,
dateofcompletionactual             :null,
file_name                          :null,
filename                           :null,
remark                             :null,
usera                              :null,
userb                              :null,
userc                              :null,
userd                              :null,
submittedby                        :null,
submitdate                         :null,
status                             :"Ongoing",
oldscheme_id:null,
                roadphysicals: [
                    {
                        sanction_year: null,
                             month: null,
                             roadbridgelenghth: 0,
                             //completedlengthtillnow: 0,
                             //balancelengthtillnow: 0,

                             earthwork: 0,
                             gsb: 0,
                             wbmgradetwo: 0,
                             wbmgradethree: 0,
                             busg: 0,
                             pmc: 0,
                             pcc: 0,
                             cd: 0,
                             gw: 0,
                             drain: 0,
                             physicalpercent: 0,
                             //administrativesanctionamount: 0,
                        totexpenditure:0,
                            // agreementamount: 0,
                      //  totexpenditureprevious: 0
                        submittedby:null
                    }
                ],


        },
        months:["APRIL","MAY","JUNE","JULY","AUGUST","SEPTEMBER","OCTOBER","NOVEMBER","DECEMBER","JANUARY","FEBRUARY"],
        typelist:["Repair","Stregthning"],
        divisionlist:null,
        districtList:null,
        blocklist:[],
        panchayatList:[],
        villageList:[],
        busy: false,
        subdivisionList: null,
        assemblylist: null,
        oldshemes:[]
    },
    mounted(){
        axios.all([
                  
        axios.get("/api/common/Division"),
        axios.get("/api/common/District"),
            axios.get("/api/common/assembly"),
       

        ]).then(axios.spread((data1, data2,data3) => {
            this.divisionlist = data1.data;
            this.districtList = data2.data;
            this.assemblylist = data3.data;
        }))
       .catch(error => alert(error))
       .finally(() => {                
       });
    },
    methods: {
        FillSubdivision() {
            axios.get('/api/Common/subdivision?division=' + this.completeenrty.division).then(data => {
                this.subdivisionList = data.data;
            })
        },
        FillBlock(){
            axios.get('/api/Common/Block?district=' + this.completeenrty.district).then(data => {
                this.blocklist=data.data;
            })
        },
        FillPanchayat(){
            axios.get('/api/common/panchayat?district=' + this.completeenrty.district + '&block=' + this.completeenrty.block).then(data => {
                this.panchayatList=data.data;
            })
        },
        FillVillage(){
            axios.get('/api/common/village?district=' + this.completeenrty.district + '&block=' + this.completeenrty.block + '&panchayat=' + this.completeenrty.panchayat).then(data => {
                this.villageList=data.data;
            })

        },
        addPhyscial() {
            this.completeenrty.roadphysicals.push({
                month: null,
                roadbridgelenghth: 0,
                completedlengthtillnow: 0,
                balancelengthtillnow: 0,
                earthwork: 0,
                gsb: 0,
                wbmgradetwo: 0,
                wbmgradethree: 0,
                busg: 0,
                pmc: 0,
                pcc: 0,
                cd: 0,
                gw: 0,
                drain: 0,
                physicalpercent: 0,
                administrativesanctionamount: 0,
                agreementamount: 0,
                totexpenditureprevious: 0

            });
        },
        remove(index){
            this.completeenrty.roadphysicals.splice(index, 1)
        },
        saveData() {
            if (this.busy == true) return;
            this.busy = true;
            var formData = new FormData();
            var imagefile = document.querySelector('#filename_c');
            formData.append("txtname", imagefile.files[0]);
            axios.post('/OngoingScheme/UploadFiles', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            }).then(data1 => {
                if (data1.data.status == "OK") {
                    this.completeenrty.filename = data1.data.file;
                    // alert(data1.data.file);
                    this.busy = false;
                    var data = JSON.parse(JSON.stringify(this.completeenrty))
                    console.log(data)
                    axios.post("/api/OngoingScheme/Save", data).then(d => {
                        if (d.data.status == "OK") {
                            alert("Data saved")
                            window.location.href = '/OngoingScheme/Create'
                        } else {
                            alert(d.data.Message)
                        }


                    })
                } else {
                    alert(data1.data.Message)
                }

            })




        },
        rem(){
            this.completeenrty.roadphysicals = []
        },
        popUp() {
            //alert(this.completeenrty.type)
            //alert(this.completeenrty.division)
            var d = this.completeenrty.division;
            var sd = this.completeenrty.subdivision;
            //alert(this.completeenrty.subdivision)
            if (this.completeenrty.type == "Strengthening of REO Road" || this.completeenrty.type == "Repair and Maintenance of REO Road") {
               
                // if (this.oldshemes.length == 0) {
                axios.get(`/api/completeentry/?division=${d}&subdivision=${sd}`).then(d => this.oldshemes = d.data)
                this.$refs["data"].show();
               // axios.get('/api/completeentry/').then(d => this.oldshemes = d.data)
                // }
                
            } else {
                this.completeenrty.oldscheme_id = null;
            }
           
            //ajax
        },
        selectRefrence(refrence) {

            console.log(JSON.stringify(refrence));
            this.completeenrty.oldscheme_id = refrence.oldscheme_id;
            this.$refs["data"].hide();

        }
    }




})