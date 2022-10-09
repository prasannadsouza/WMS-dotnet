import { useTrackedGlobalState, useUpdateGlobalState } from '../utilities/store';
import { useEffect, useState } from 'react';

export const FetchData = () => {

    type forecast = {
        date: Date,
        temperatureC: number,
        temperatureF: number,
        summary: string,
    };

    const [model, setModel] = useState({ forecasts: [], loading: true});
    
    const populateWeatherData = async () => {
        const response = await fetch('weatherforecast');
        const response1 = await fetch('weatherforecast/GetConfigSetting');
        const data = await response.json();
        setModel({ forecasts: data, loading: false })
    }

    const updateAppConfig = useUpdateGlobalState();
    const appConfig = useTrackedGlobalState();
    let title = appConfig.globalStrings!.fetchdata;
        

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

  

