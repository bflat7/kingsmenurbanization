import React from 'react';
import { Link } from 'react-router-dom';
import { makeStyles, AppBar, Typography } from '@material-ui/core';
import Toolbar from "@material-ui/core/Toolbar";

const useStyles = makeStyles((theme) => ({
  root: {
    flexGrow: 1,
  },
  title: {
    flexGrow: 1,
    textAlign: "left",
    marginLeft: "2rem",
  },
  lnk: {
    color: "#000000",
    marginLeft: ".5rem",
    marginRight: ".5rem",
    fontFamily: "Arial",
    textDecoration: "None",
  }
}));

export function NavMenu() {
  const classes = useStyles();

  return (
    <div className={classes.root}>
      <AppBar color="secondary" position="static">
        <Toolbar>
          <Typography variant="h6" className={classes.title}>
            <Link className={classes.lnk} to="/">Urbanization Data</Link>
          </Typography>
          <Typography variant="body1">
            <Link className={classes.lnk} to="/">Home</Link>
            <Link className={classes.lnk} to="/urbanization-data">Data</Link>
          </Typography>
        </Toolbar>
      </AppBar>
    </div>
  );
}
