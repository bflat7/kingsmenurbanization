// import * as fetchMock from "fetch-mock";
import fetchMock from "fetch-mock/es5/client";
import UrbanizationService from "./UrbanizationService";

describe("UrbanizationService test", () => {
    const urbService = new UrbanizationService();
    
    beforeEach(() => {
        fetchMock.reset();
    });

    describe("getTotalUrbanizationByStateDataCount tests", () => {
        const countEndpoint = "urbanization/count";
        
        it("throws error when not success code", () => {
            fetchMock.mock(countEndpoint, 500);
            const promise = urbService.getTotalUrbanzationByStateDataCount(); 
            expect(fetchMock.called(countEndpoint)).toBeTruthy(); 
            expect(promise).rejects.toBeTruthy();
        });
    
        it("returns expected count when success code", async () => {
            const expectedCount = 10;
            fetchMock.mock(countEndpoint, expectedCount.toString());
            const data = await urbService.getTotalUrbanzationByStateDataCount();
            expect(fetchMock.called(countEndpoint)).toBeTruthy();
            expect(data).toBe(expectedCount);
        });
    })

    describe("getUrbanizationByStateData tests", () => {
        const page = 1, rowsPerPage = 1, orderby = "column", order = "asc";
        const dataEndpoint = `urbanization?page=${page}&rowsPerPage=${rowsPerPage}&orderBy=${orderby}&order=${order}`;

        it("throws error when not success code", () => {
            fetchMock.mock(dataEndpoint, 500);
            const promise = urbService.getUrbanizationByStateData(page, rowsPerPage, orderby, order);
            expect(fetchMock.called(dataEndpoint)).toBeTruthy();
            expect(promise).rejects.toBeTruthy();
        });

        it("returns expected data when success code", async () => {
            const stateObject = {
                stateName: "North Carolina",
                stateFips: 10,
                gisJoin: "join",
                latLong: "latLong",
                population: 10,
                urbanIndex: 1,
            }
            fetchMock.mock(dataEndpoint, [stateObject]);
            const data = await urbService.getUrbanizationByStateData(page, rowsPerPage, orderby, order);
            expect(fetchMock.called(dataEndpoint)).toBeTruthy();
            expect(data).toStrictEqual([stateObject]);
        });
    });
});

