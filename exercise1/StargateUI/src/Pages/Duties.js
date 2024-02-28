import * as React from 'react';
import { MakeGetRequest, MakePostRequest } from '../Utility/Api';
import Box from '@mui/material/Box';
import { DataGrid } from '@mui/x-data-grid';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';

const columns = [
    {
      field: 'name',
      headerName: 'Name',
      width: 150,
      editable: false,
    },
    {
      field: 'rank',
      headerName: 'Rank',
      width: 150,
      editable: false,
    },
    {
      field: 'dutyTitle',
      headerName: 'Duty Title',
      width: 110,
      editable: false,
    },
    {
      field: 'dutyStartDate',
      headerName: 'Duty Start Date',
      sortable: false,
      width: 250
    },
    {
        field: 'dutyEndDate',
        headerName: 'Duty End Date',
        sortable: false,
        width: 250
      },
  ];

export default function Duties() {
    const [rows, setRows] = React.useState([]);
    const [name, setName] = React.useState("");
    function parseRows(data){
        var duties = JSON.parse(data).astronautDuties;
        console.log(duties);
        var tempRows = [];
        duties.map((x, i) => tempRows.push({
            id: i, 
            name: name,
            dutyTitle: x.dutyTitle, 
            rank: x.rank, 
            dutyEndDate: x.dutyEndDate, 
            dutyStartDate: x.dutyStartDate}));
        setRows(tempRows);
    }
    function getDuties(){
        MakeGetRequest('AstronautDuty/'+ name, null, (result) => {
            parseRows(result);
        })
    }
    return (
        <div>
            <center>
                Search for Duties by astronaut name
                <br></br>
                <TextField required onChange={event => setName(event.target.value)} id="outlined-basic" label="Name" variant="outlined" />
                <Button onClick={x => getDuties()} variant="contained">Search</Button>

            </center>
            <br/>
            <center>
                <Box sx={{ height: 400, width: '80%' }}>
                <DataGrid
                    rows={rows}
                    columns={columns}
                    initialState={{
                    pagination: {
                        paginationModel: {
                        pageSize: 5,
                        },
                    },
                    }}
                    pageSizeOptions={[5]}
                    disableRowSelectionOnClick
                />
                </Box>  
            </center>
            <br/>
            
        </div>
    )
}