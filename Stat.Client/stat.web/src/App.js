import 'bootstrap/dist/css/bootstrap.min.css';
import Dropdown from 'react-bootstrap/Dropdown';
import Container from 'react-bootstrap/Container';
import React, { useState, useEffect } from 'react';
import { requestUri } from './config';
import axios from 'axios';
import './App.css';

function App() {
  const [leagues, setLeagues] = useState([]);
  const [matches, setMatches] = useState([]);
  const [brands, setBrands] = useState([]); // New state for brands
  const [selectedLeague, setSelectedLeague] = useState('Select League');
  const [selectedBrand, setSelectedBrand] = useState('Select Brand');
  const [selectedLeagueId, setSelectedLeagueId] = useState(null);
  const [selectedBrandId, setSelectedBrandId] = useState(null);

  useEffect(() => {
    axios.get(`${requestUri}/league`)
        .then(response => {
          setLeagues(response.data);
        })
        .catch(error => {
          console.error('There was an error!', error);
        });
  }, []);

  const fetchBrandMatches = () => {
    if (selectedLeagueId) { // Only fetch if a league is selected
      let requestUrl = `${requestUri}/match/${selectedLeagueId}`;
      if (selectedBrandId) { // If a brand is also selected, add it to the request URL
        requestUrl += `/${selectedBrandId}`;
      }

      axios.get(requestUrl)
          .then(response => {
            const matchesWithBrandData = response.data.map(match => {
              // Assuming the response includes brand data
              match.brand = match.brand || {};
              return match;
            });
            setMatches(matchesWithBrandData);
          })
          .catch(error => {
            console.error('There was an error!', error);
          });
    }
  }

  useEffect(() => {
    axios.get(`${requestUri}/Brand/GetBrandList`) // Fetch brands from /brand endpoint
        .then(response => {
          setBrands(response.data);
        })
        .catch(error => {
          console.error('There was an error!', error);
        });
  }, []);

  const clearSelection = () => {
    setSelectedLeague('Select League');
    setSelectedBrand('Select Brand');
    setSelectedLeagueId(null);
    setSelectedBrandId(null);
    setMatches([]); // Clear the matches

  }

  return (
      <Container className="App">
        <h1 className="text-center my-4">Select League</h1>
        <div className="d-flex justify-content-center">

          <Dropdown onSelect={(key, event) => {
            setSelectedLeagueId(event.target.id);
            setSelectedLeague(key);
          }}>
            <Dropdown.Toggle variant="success" id="dropdown-basic">
              {selectedLeague} {/* Display selected league text */}
            </Dropdown.Toggle>

            <Dropdown.Menu>
              {leagues.map((league, index) => (
                  <Dropdown.Item key={index} eventKey={league.name} id={league.id}>{league.name}</Dropdown.Item>
              ))}
            </Dropdown.Menu>
          </Dropdown>
          <Dropdown onSelect={(key, event) => {
            setSelectedBrandId(event.target.id);
            setSelectedBrand(key);
          }}>
            <Dropdown.Toggle variant="success" id="dropdown-brand">
              {selectedBrand} {/* Display selected brand text */}
            </Dropdown.Toggle>

            <Dropdown.Menu>
              {brands.map((brand, index) => (
                  <Dropdown.Item key={index} eventKey={brand.name} id={brand.id}>{brand.name}</Dropdown.Item>
              ))}
            </Dropdown.Menu>
          </Dropdown>

          <button className="btn btn-primary fetch-button" onClick={fetchBrandMatches}>Fetch Matches</button>
          <button className="btn btn-secondary" onClick={clearSelection}>Clear Selection</button>
        </div>
        <div>
          <h2>Upcoming Matches</h2>
          {matches.map((match, index) => (
              <div key={index}>
                <p style={{fontWeight: 'bold', backgroundColor: 'grey', color: 'white'}}>Match
                  on {new Date(match.date).toLocaleDateString()}</p>
                <div>
                  <p>
                    {match.homeTeam.brand && <span style={{
                      backgroundColor: match.homeTeam.brand.primaryColor,
                      width: '20px',
                      height: '20px',
                      display: 'inline-block',
                      marginRight: '10px'
                    }}></span>}
                    {match.homeTeam.name}
                    {match.homeTeam.brand ? `(Brand: ${match.homeTeam.brand.name})` : ''}
                  </p>
                </div>
                <div>
                  <p>
                    {match.awayTeam.brand && <span style={{
                      backgroundColor: match.awayTeam.brand.primaryColor,
                      width: '20px',
                      height: '20px',
                      display: 'inline-block',
                      marginRight: '10px'
                    }}></span>}
                    {match.awayTeam.name}
                    {match.awayTeam.brand ? `(Brand: ${match.awayTeam.brand.name})` : ''}
                  </p>
                </div>

              </div>
          ))}
        </div>
      </Container>
  );

}

export default App;