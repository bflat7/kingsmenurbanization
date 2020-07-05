// import DataTable, {SortOrderEnum, TableHeaders} from "./DataTable";
import DataTable from "./DataTable";
import {SortOrderEnum, TableHeaders} from "./DataTable";
import fetchMock from "fetch-mock/es5/client";
import React from 'react';
import {cleanup, render, wait} from "@testing-library/react";

const stateObject = {
    stateName: "North Carolina",
    stateFips: 10,
    gisJoin: "join",
    latLong: "latLong",
    population: 10,
    urbanIndex: 1,
}

afterEach(cleanup);

describe("DataTable component tests", () => {
    function renderDatatable() {
        render (
            <DataTable/>
        )
    }
    
    beforeEach(() => {
        fetchMock.reset();
    });

    it("renders without crashing", () => {
        fetchMock.mock("urbanization/count", "500");
        fetchMock.mock("begin:urbanization?page=", [stateObject])
        renderDatatable();
    });
    
    it("shows both error messages", () => {
        fetchMock.mock("urbanization/count", 500);
        fetchMock.mock("begin:urbanization?page=", 500);
        const { debug } = render(<DataTable/>);
        debug();
    })
});