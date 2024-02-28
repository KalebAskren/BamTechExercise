import React, { useState } from 'react'
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import { MakePostRequest } from '../Utility/Api';

export default function CreateDuty() {
  const[name, setName] = useState("");
  const[rank, setRank] = useState("");
  const[title, setTitle] = useState("");
  const[startDate, setStartDate] = useState("");
  function createDuty() {
      MakePostRequest('AstronautDuty/', JSON.stringify({name: name, rank: rank, dutyTitle: title, dutyStartDate: startDate}), (response) => {
          alert(response);
      });
  }
  return (
  <div>
      <div>Create a New Duty</div>
      <br/>
      <Box
          component="form"
          sx={{
              '& > :not(style)': { m: 1, width: '25ch' },
          }}
          noValidate
          autoComplete="off"
          >
          <TextField required onChange={event => setName(event.target.value)} id="outlined-basic" label="Name" variant="outlined" />
          <TextField required onChange={event => setRank(event.target.value)} id="outlined-basic" label="Rank" variant="outlined" />
          <TextField required onChange={event => setTitle(event.target.value)} id="outlined-basic" label="Title" variant="outlined" />
          <TextField required onChange={event => setStartDate(event.target.value)} id="outlined-basic" label="Start Date" variant="outlined" />
          <Button onClick={x => createDuty()} variant="contained">Submit</Button>
      </Box>
  </div>

  )
}
