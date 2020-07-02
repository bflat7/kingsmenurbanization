import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>State Urbanization</h1>
            <p>The data being shown here was taken from <a href='https://fivethirtyeight.com/'>538</a> and shows the urbanization index for counties. Some of the more complex data representations are explained below:</p>
        <ul>
          <li>State Fips: State Federal Information Processing Standard</li>
          <li>GIS Join: Geographical Information System from census tract of the 2010 census</li>
          <li>Urban Index: A calculation of how urban or rural a given area is</li>
        </ul>
      </div>
    );
  }
}
