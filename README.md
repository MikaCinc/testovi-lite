# Testovi Lite edition

![Homepage](Slike/home.jpg)

[Testovi - Full edition](https://testovi-inc.netlify.app/)
---
[Mihajlo - Portfolio](https://www.mihajlo.engineer/)

Testovi Lite sadrže 3 glavna entiteta: 
- Spojnica
- Pitanje
- Tag

## Spojnica

![Spojnica model](Slike/s_model.jpg)
![Spojnica CRUD](Slike/s_rute.jpg)

Posebna JS klasa upravlja prikazom "Pregleda" spojnice na homepage-u, i prikazom i funkcionalnostima same spojnice na posebnoj stranici.

![Spojnica Page](Slike/spojnica_page.jpg)

Moguće je dodavati i brisati tagove, menjati naziv i prioritet/težinu spojnice u > Edit modu
Naravno, moguće je dodavati i brisati pitanja iz spojnice. Dodavanje pitanja može biti:
- Novo pitanje, tako što se unesu pitanje i njegov odgovor na licu mesta
- Izbor jednog od svih dostupnih pitanja iz baze a koje već nije deo spojnice

Spojnica se rešava kao u Slagalici, odozgo na dole, pitanej po pitanje a izborom 
odgovarajućeg odgovora iz desne kolone. U zavisnosti od stanja spojnice, tabela
sa pitanjima i odgovorima poprima odgovarajuće stilove nakon svakog rendera.
Klikom na dugme "Reset" moguće je reshufflovati pitanja i odgovore i krenuti iz početka.

Spojnica takođe ima i "NEW mod", u koji ulazi ako se na homepage-u klikne na dugme
"Nova spojnica", tada se unosi naziv i prioritet pre objave spojnice rutom
"Spojnica/DodajSpojnicu" a klikom na dugme "Objavi spojnicu". Nakon objave spojnica
ulazi u regularan mod gde dalje mogu da se unose tagovi i pitanja...

## Pitanja

![Pitanje model](Slike/p_model.jpg)
![Pitanje CRUD](Slike/p_crud.jpg)

Pitanje je sastavni deo spojnice ali je moguće interagovati sa njima na homepage-u.
Listu pitanja kontrolišu JS klase za pitanja.
Moguće je dodati novo pitanje, unošenjem pitanja i odgovora na samom vrhu liste sa
pitanjima.
Moguće je unošenje i provera odgovora i prikazivanje tačnog odgovora na svakom
"Pregledu" pitanja i to takođe kontroliše odgovarajuća JS klasa za pitanje.

## Tagovi
Tagovi 