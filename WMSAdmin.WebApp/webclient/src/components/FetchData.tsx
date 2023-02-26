import { AppSlice, useAppDispatch, useTrackedGlobalState, useUpdateGlobalState } from '../utilities/store';
import { useEffect, useState } from 'react';
import { ResponseData } from '../entities/entities';
import { APIParts } from '../entities/constants';
import { Utility } from '../utilities/utility';
import { useNavigate } from 'react-router-dom';

export const FetchData = () => {

    const appConfig = useTrackedGlobalState();
    const updateAppConfig = useUpdateGlobalState();
    const appState = useTrackedGlobalState();
    const navigate = useNavigate();
    const dispatch = useAppDispatch();
    const { setSessionIsAuthenticated } = AppSlice.actions;

    type forecast = {
        date: Date,
        temperatureC: number,
        temperatureF: number,
        summary: string,
    };

    const [model, setModel] = useState({ forecasts: [], loading: true});
    
    const populateWeatherData = async () => {

        const response = await Utility.GetData<forecast[]>(APIParts.TEST , undefined, { data: null });
        if (Utility.handleAllErrors(appState, response.errors, dispatch, setSessionIsAuthenticated, updateAppConfig)) return;
        
        
        const response1 = await Utility.GetData<forecast[]>(APIParts.TEST + "GetConfigSetting", undefined, { data: null });
        if (Utility.handleAllErrors(appState, response1.errors, dispatch, setSessionIsAuthenticated, updateAppConfig)) return;
        setModel({ forecasts: response as forecast[], loading: false })
    }

    
    let title = appConfig.generalString!.fetchData;
        

    useEffect(() => {
        populateWeatherData();
        if (appConfig.currentTitle !== title) updateAppConfig((prev) => ({ ...prev, currentTitle: title }));
    }, []);

    const renderForecastsTable = (forecasts: forecast[]) => {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Temp. (C)</th>
                        <th>Temp. (F)</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    {forecasts.map(forecast =>
                        <tr key={forecast.date.toString()}>
                            <td>{forecast.date.toString()}</td>
                            <td>{forecast.temperatureC}</td>
                            <td>{forecast.temperatureF}</td>
                            <td>{forecast.summary}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    let contents = model.loading
        ? <p><em>Loading...</em></p>
        : renderForecastsTable(model.forecasts);

    return (
        <div>
            <h1 id="tabelLabel" >Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );
};

  

