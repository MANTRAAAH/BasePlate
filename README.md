# 🚀 EXECUTIVE PROJECT BLUEPRINT
**Progetto:** Turnkey Digital Ecosystem & SaaS (Smart Table, MDM, Web)
**Settore:** Food & Beverage / Hospitality (PMI, Ristoranti, B&B)
**Modello di Business:** White-Label B2B SaaS

---

## 📊 1. Modello Finanziario e Go-to-Market
Il prodotto è ingegnerizzato come una soluzione SaaS multi-tenant scalabile. L'investimento nello sviluppo del "Framework" iniziale (Core Engine) viene ammortizzato rapidamente, trasformando le successive vendite in margine netto (rendita ricorrente).

*   **Pricing Standard al Cliente (Total Contract Value Anno 1: € 3.500)**
    *   **Setup Iniziale (€ 2.000 una tantum):** Copre l'onboarding, la configurazione del dominio, il setup del sito vetrina personalizzato, e l'attivazione del workspace MDM e della PWA di sala.
    *   **Canone Ricorrente (€ 1.500/anno):** Licenza d'uso del software SaaS, hosting Cloud, manutenzione sistemistica, notifiche push e aggiornamenti continui. Genera un ARR (Annual Recurring Revenue) di € 125/mese per istanza.
*   **Strategia di Acquisizione:** Proposizione di un ecosistema digitale "chiavi in mano" (Sito ad alte performance + Software di Sala) che disintermedia le OTA (per il ricettivo) e abbatte i tempi morti nel servizio al tavolo, generando un ROI tangibile e immediato per il titolare.

---

## ⚙️ 2. Architettura IT (SaaS Engine Multi-Tenant)
L'infrastruttura è progettata per massimizzare la flessibilità, minimizzare i costi di esecuzione e azzerare il rischio di *Data Leakage* tra i vari locali.

*   **Approccio Database:** Multi-Tenant a Database Singolo (Shared Schema).
*   **Backend (API Core):** C# / .NET Core. Gestisce l'orchestrazione, la validazione e la sicurezza.
*   **Isolamento Dati (Security by Design):** Implementazione rigorosa dei **Global Query Filters di EF Core**. Ogni query al database applica automaticamente il filtro sul `TenantId`, prevenendo nativamente l'esposizione accidentale di dati tra ristoranti diversi.
*   **Frontend (White-Label):** Angular (con Tailwind CSS). Il codice è unico; i temi, i loghi e i colori vengono iniettati dinamicamente tramite file di configurazione per ogni Tenant.
*   **Performance & Caching:** Redis utilizzato come layer di cache per il catalogo menu, azzerando il carico di lettura sul database PostgreSQL durante i picchi di affluenza in sala (effetto "Noisy Neighbor").
*   **Storage Risorse:** Object Storage (Azure Blob / S3) strutturato a compartimenti stagni per Tenant (es. `/tenant-ID/assets/`) per la gestione di immagini dei piatti e loghi.
*   **Infrastruttura & DevOps:** Containerizzazione Docker e pipeline CI/CD per rilasci globali simultanei su tutto il parco clienti.

---

## 📦 3. Work Breakdown Structure (Moduli del Sistema)

### Modulo A: Front-Office Cliente (Conversion & UX)
*   **Sito Vetrina Personalizzato:** Sito pubblico ad alte prestazioni (Lighthouse score ~100) ottimizzato per la SEO locale.
*   **QR Gateway & Web App Menu:** Accesso istantaneo tramite 4G/5G al tavolo senza installazione. Identificazione automatica del Tenant e del numero del tavolo tramite parametro URL.
*   **Wishlist Engine:** Menu navigabile dinamicamente con badge per allergeni calcolati in tempo reale e logiche di cross-selling.

### Modulo B: Back-Office Sala (Efficienza Operativa)
L'interfaccia è progettata per risolvere le criticità pratiche sul piano operativo (viaggi a vuoto, device personali, turni lunghi).
*   **PWA (Progressive Web App) BYOD:** Interfaccia installabile sui dispositivi personali dello staff tramite browser. Zero costi hardware per il ristoratore.
*   **Ergonomia e Batteria:** Design esclusivamente in **Dark Mode** per massimizzare il risparmio energetico dei device durante i turni di servizio.
*   **Notifiche Push Prioritizzate (SignalR):** Motore real-time in C# che instrada le chiamate dai tavoli ("Ordine Pronto", "Conto", "Assistenza") ai device in sala, associate a feedback acustici differenziati per minimizzare l'attrito cognitivo.

### Modulo C: Master Data Management (PIM Gestionale)
Pannello Admin per il ristoratore, basato su un solido schema relazionale per garantire l'integrità del catalogo.
*   **Dizionario Normalizzato:** Tabelle di lookup per Ingredienti, Categorie e Tipologie di Cottura (es. "Pizza a 3 cotture").
*   **Gestione Allergeni Automatica:** Relazione molti-a-molti tra Ingredienti e Allergeni. Se un piatto include un ingrediente a rischio, il sistema propaga il warning su tutti i frontend automaticamente.
*   **Digital Sommelier:** Scheda prodotto estesa consultabile dalla sala per fornire risposte precise sulle preparazioni ai clienti esigenti.

### Modulo D: Integrazioni Fiscali (Middleware Asincrono)
*   **Pattern Adapter:** Architettura predisposta per integrarsi con sistemi esterni senza inquinare il core.
*   **Shadow Comanda:** Gestione del ciclo di vita dell'ordine a stati (Bozza -> Modifica -> Da Saldare) salvata nel database.
*   **Integrazione Cloud-to-Cloud / Local Worker:** Background services in .NET Core per impacchettare l'ordine chiuso e inviarlo in modo asincrono (fire-and-forget con retry policy) a servizi di cassa o stampanti telematiche LAN, senza mai bloccare la UI dell'operatore di sala.
