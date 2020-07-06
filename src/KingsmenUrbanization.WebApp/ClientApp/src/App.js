import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import DataTable from './components/DataTable';
import { ThemeProvider, createMuiTheme } from '@material-ui/core';

const theme = createMuiTheme({
  palette: {
    primary: {
      main: "#a7c6da",
    },
    secondary: {
      main: "#FFFFFF",
    }
  },
  typography: {
    body1: {
      fontWeight: 100,
      fontSize: 14,
    },
  }
});

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <ThemeProvider theme={theme}>
        <Layout>
          <Route exact path='/' component={Home} />
          <Route path='/urbanization-data' component={DataTable} />
        </Layout>
      </ThemeProvider>
    );
  }
}
