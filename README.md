# Zápočtový program 1.roč. BC, LS 2020/21 NPRG031 - Gabriela Závodská

Verzia hry [Motos](https://en.wikipedia.org/wiki/Motos)  

## Špecifikácia
 
Hlavná myšlienka [hry] by ostala, no obmedzila by som sa na nejakých 3 až 5 levelov, a pár objektov (Motos, Red Pupas, Blue Pupas, Nabicons, Spiruses, Beecons), pri ktorých sa budem snažiť udržať parametre (váhu, rýchlosť a body) ako špecifikované na wiki. Hracia plocha by bola pre jednoduchosť vždy súvislá bez možnosti spraviť do nej dieru, keďže vynechám aj schopnosť Motos-u robiť skoky. Tak isto by som vynechala aj schopnosť power boostu. Hra si bude pamätať highscore (asi tam nebude veľa možností, takže pri highscore si bude pamätať aj čas za ktorý sa dané skóre dosiahlo).

## Čo sa nespravilo

Ako som to dostala aj v odpovedi na mail so špecifikáciou, bolo to veľa práce. Chcela som postupovať postupne, tak som začala len jedným levelom a pri ňom som aj skončila. Tým pádom som prvky vystupujúce v hre tiež obmedzila na 4: Motos, Red Pupas, Blue Pupas a Beecons. Zároveň bolo príliš teplo na to, aby som prepočítavala rýchlosť z km/h do px/timer_Tick, preto som rýchlosť aj váhu určila tak, aby to vyzeralo počas hrania rozumne (špecifiká neskôr). Keďže má hra len jeden level, namiesto skóre si pamätá čas v sekundách.  

## Používanie


### Spustenie programu

Je potrebné skompilovať kód a následne program spustiť.  
Objaví sa okno s úvodnou obrazovkou obsahujúcou tlačítko `START`, po jeho stlačení sa hra spustí.

### Ovládanie hry

Hra sa spúšťa pomocou tlačítka `START`, kliknutím pravého tlačítka myši alebo stlačením klávesy `Enter`.  
Ovládateľný prvok hry je takzvaný Motos, ktorý vyzerá ako raketka a po spustení hry sa nachádza v strede hracej plochy. Vie sa pohybovať štyrmi smermi (hore, doľava, dole a doprava) a ovláda sa klávesami `wasd`.  
Po výhre resp. prehre sa objaví `messageBox`, po jeho zatvorení alebo kliknutí na tlačítko `OK` sa hra vráti na úvodnú obrazovku. 

### Cieľ hry

Na to, aby hráč vyhral, musí svojím pohybom zhodiť z hracej plochy všetky ostatné prvky a následne sa vrátiť do stredu hracej plochy. Snaha je dosiahnuť to čo najrýchlejšie, keďže najlepšie tri skóre ostávajú v pamäti programu.  
Ak sa za okraj hracej plochy posunie väčšia časť ako polovica Motosu, teda spadne z hracej plochy, nastane prehra - nová hra sa potom obnovuje so všetkými prvkami.

### Prvky hry

Základným prvkom hry je štvorcová hracia plocha, ktorá je zložená z ružových a žltých štvorčekov, položená na čiernom pozadí.  
Pohyblivé prvky hry sú následujúce:
* __Motos__: Ovládateľný prvok hry, pohybuje sa základnými štyrmi smermi, no po náraze vie odskočiť aj šikmo. V pohybe je len keď sa drží šípka alebo po zrážke s iným pohyblivým prvkom. Vyzerá ako raketka. Váži 5 imaginárnych jednotiek váhy (ijv).  
* __Red Pupa__: Červená guľa (kruh) váži 5 ijv a má maximálnu rýchlosť 6 px/tick. Svoj smer a rýchlosť pohybu mení náhodne po určitom časovom intervale alebo keď sa dostane príliš blízko okraju hracej plochy samostaným pohybom. Za jej zhodenie z hracej plochy patrí 300 bodov.  
* __Blue Pupa__: Modrá guľa (kruh) váži 6 ijv a má maximálnu rýchlosť 5 px/tick. Smer a rýchlosť pohybu mení podobne ako jej červený ekvivalent. Za zhodenie z hracej plochy hráč získa 400 bodov.  
* __Beecon__: Červeno-sivý diamant nemá samostatný pohyb, pohne sa až keď doňho narazí Motos. Tento prvok má 100 ijv a po jeho zhodení z hracej plochy sa skóre zvyšuje o 1000.  

Je potrebné dodať, že prvky reagujú len na zrážky s Motos-om, inak na seba nepôsobia (jeden preletí ponad druhý).  
Ďalšie elementy, ktoré už ale aktívne nevstupujú do hry, sú počítadlo skóre a časomiera, umiestnené pod hracou plochou.

## Popis programu

### Komponenty WinForms-u

Hlavná komponenta programu je okno `Form`, ktoré spracováva stlačenie a spustenie klávesy. Má dva hlavné výzory, odvýjajúce sa od stavu hry (dodatočne si uvedomujem, že toto nie je úplne presný popis, keďže objekt hry sa vytvára až po stlačení `START`-u). Ak je stav hry `nezacala`, zobrazuje sa úvodné okno s následujúcimi komponentami:
* Label `label1`, ktorý obsahuje "tabuľku" s highscores
* Picturebox `pictureBox2`, ktorého atribút Image je nastavený na "logo" hry (robila som ho ja, lebo why not)
* Button `button1` slúžiaci na spustenie hry  

Po spustení hry, teda kliknutí na button `START`, sa vytvorí nový objekt hry v stave `bezi` a zmení sa výzor okna - sú na ňom komponenty:
* PictureBox `pictureBox1`, na ktorý sa vykresluje hra - o spôsobe vykreslovania neskôr
* Label `label2`, na ktorý sa vypisuje skóre
* Label `label3`, na ktorý sa vypisuje čas, ktorý ubehol od spustenia hry  

Za periodické prekreslovanie zodpovedá Timer `timer1`, pri jeho evente `Tick` sa v závislosti na stave hry prevedú príslušné akcie.

### Objektová dekompozícia

#### Form

Hlavným objektom je objekt `Form`, ktorý má nasledujúe atribúty:
* `Graphics g`: slúži na vykreslovanie prvkov hry na obrázok
* `Bitmap pozadie` a `Bitmap erease`: obe bitmapy sú vytvorené z toho istého obrázku `pozadie.png` - na `pozadie` sa vykreslujú prvky hry a pri každom `Tick`-u časovača sa premazáva bitmapou `erease`. Hracia plocha na tejto bitmape je určená štvorcom daným bodmi [50; 60] a [450; 460].
* `Hra hra`: objekt reprezentujúci hru, má v sebe pohyb prvkov a pod. (detaily neskôr)
* `TimeSpan ubehlo`: premenná na zistenie času od spustenia hry po aktuálny moment hry
* `Highscore high`: objekt na čítanie a zapisovanie do súboru obsahujúceho top 3 skóre
* `Sipka stlacenaSipka`: premenná obsahujúca informáciu o aktuálne stlačenej šípke - nastavuje sa funkciami `Form1_KeyDown` a `Fom1_KeyUp`

Najdôležitejšia metóda je `timer1_Tick`, ktorá refreshuje hru pri každom ticku. V závislosti na stave hry:
* Ak hra beží, premaže sa obrázok, refreshne sa hra a prepočíta sa skóre a čas.
* Ak nastala výhra resp. prehra, ukáže sa MessageBox so správou o stave hre. Po jeho zatvorení sa hra hra vráti na úvodný screen pomocou funkcie `Restart()`, ktorá vlastne vygeneruje nové okno a staré zatvorí.  

#### Highscore

Táto trieda slúži na čítanie zo a zapisovanie do súboru `highscores.txt`. Tento súbor má veľmi jednoduchú štruktúru, na troch riadkoch má uložené tri najlepšie (najmenšie) dosiahnuté skóre (najkratšie časy za ktoré sa dosiahla výhra) zoradené od najlepšieho, úplne na začiatku nastvené na tri nuly. Má stringový atribút `highscores`, v ktorom sú uložené informácie o 3 highscores ako textový reťazec s číselnými hodnotami na zvlášť riadkoch. Pri spustení programu sa súbor prečíta pomocou `NacitajSkore()`, aby sa mohlo na úvodnom screene vypísať. Po tom, ako nastane výhra, sa zavolá funkcia `PridajAktualneSkore()`, ktorá kontroluje, či má dané skóre pridať do súboru, a ak áno, tak kam `ZapisSkore()`, ktorá prepíše súbor `highscores.txt`.

#### Hra

Objekt hry obsahuje tri zoznamy: `xove_suradnice` a `yove_suradnice` typu float, ktoré majú rolu len pri počiatočnom generovaní prvkov - aby sa na začiatku navzájom neprekrívali. Tretí zoznam je zoznam neovládateľných Prvkov `prvkyBezMotosu`, do ktorého sa pridávajú prvky pri vytvorení a odoberajú po tom, ako vypadli z hracej plochy. Ďalším atribútom hry je `motos` - ovládateľný prvok (o motose a ostatných neskôr).  
Hra má aj svoj Graphics `graphics`, ktorý je totožný s Graphicsom celého Formu, predáva sa konštruktoru. Medzi ďalšie atribúty patria `stav` typu Stav, `casomer` typu Stopwatch na meranie času a integerové `skore` na počítanie skóre.  
Hlavná metóda hry je `TimerUpdate()`, ktorá sa zavolá pri ticku Timeru, a vrámci ktorej sa zavolajú metódy na pohyb a vykreslenie prvkov, na kontrolu či nenastala prehra alebo výhra, na kontrolu či nejaký prvok vyletel mimo hracej plochy a na kontrolu či nenastal náraz.  
Pohyb a následné vykreslenie prvkov spracováva každý prvok zvlášť.  
`CheckVyletel` kontroluje súradnice neovládateľných prvkov, a ak sa nejaký nachádza mimo hraciu plochu, odoberie ho zo zoznamu prvkov.  
`CheckNaraz` prechádza zoznam prvkov a kontroluje, či sa ikonka nejakého z nich neprekríva z motosom (tu môže nastať menšie vizuálne oklamanie hráča, lebo ikonky prvkov sú štvorcové obrázky, takže niekedy to môže vyzerať tak, že sa prvky nezrazili, no hra spracuje náraz). Ak náraz nastal, tak sa tento náraz spracuje metódou `SpracujNaraz()`. V nej sa aplikuje zákon o zachovaní hybnosti: z hmotnosti prvku sa vypočíta sila, ktorú vyvýja na druhý prvok, zo sily pôsobiacej na daný prvok a času pôsobenia sily (čas je pre jednoduchosť nastavený na 1) sa vypočíta zmena hybnosti a následne sa prepočíta rýchlosť prvku pomocou zmeny hybnosti.

#### Prvok

Poznámka: podľa definície abstraktnej triedy z prednášky nie je abstraktná, lebo nemá žiadnu abstraktnú metódu (pôvodne mala, ale to sa pri písaní kódu zmenilo - možno by to mala byť virtuálna trieda)
Abstraktná trieda Prvok definuje základné atribúty, ktoré má mať každý prvok, či už ovládateľný alebo nie. Tieto sú:
* boolovské atribúty `v_pohybe` a `po_náraze` sa využívajú, na určenie toho, ako má vyzerať pohyb prvku  
* floatové hodnoty `x` a `y` určujú súradnice ľavého horného rohu ikony daného prvku  
* `hmotnost` a `max_rychost` sú myslím jednoznačné z názvu, `hmotnost` sa využíva pri výpočte sily a max_rychlost pri pohybe  
* Vektorové atribúty `vektor_rychlosti`, `vektor_sily`, `vektor_zrychlenia` a `vektor_hybnosti` reprezentujú vektorové veličiny rýchlosti, sily, zrýchlenia a hybnosti, ich veľkosť je potom daná normou vektoru. Využívajú sa primárne pri výpočte v zákone o zachovaní hybnosti, no aj v pohybe jednotlivých prvkov  
* Bitmapa `ikona` obsahuje obrázok reprezentujúci daný brvok (obrázky som kreslila ja)
* integerová `hodnota` obsahuje  počet bodov, ktorý sa získa zhodením daného prvku z hracej plochy
* integerová hodnota `reakcia_po_naraze` vyjadruje čas, počas ktorého má daný prvok vykonávať pohyb, ktorý mu bol určený po náraze (to je kvôli tomu, lebo nepočítam s treciou silou a inak by bolo príliš jednoduché zhodiť prvok) a integerová premenná `od_narazu` počíta čas, ktorý ubehol od nárazu  

Metódy dané v tejto triede využíva väčšina alebo všetci jej potomkovia.  
Potomkovia triedy Prvok:

##### Motos

Motos je ovľádateľný prvok hry. Jeho hlavná metóda je metóda `Pohyb()`, ktorá na základe toho či je prvok v pohybe alebo nie, alebo po náraze alebo nie mení vektor rýchlosti a polohu motosu. Ak je prvok po náraze, kontroluje, či už ubehol čas, v ktorom reaguje motos na náraz - ak neubehol, nemôže zareagovať na stlačenú klávesu. Klávesy sa spracovávajú ak prvok nie je po náraze, alebo ak nie je v pohybe (detail: správne by tam mala byť konjunkcia, ale to mi nefungovalo, tak som si povedala, že motos reálne nebude zrýchlovať, len sa tak bude tváriť :D ). Ak je prvok v pohybe, nezávisle na tom či je alebo nie po náraze, zmenia sa súradnice podľa vektoru rýchlosti.  

##### Pupa

Spoločná trieda pre Blue Pupa a Red Pupa, keďže spôsob ich pohybu je v podstate rovnaký, líšia sa len konštanty na reakcie. Má dva atribúty navyše, a to `cas_na_zmenu`, ktorá vyjadruje cas, po ktorom Pupa náhodne zmení svoj pohyb, a `od_poslednej_zmeny`, ktorá meria čas, ktorý ubehol od poslednej zmeny pohybu. Jej metóda `Pohyb()` preto vyzerá takto:
* Keďže tieto prvky sú vždy v pohybe, ich súradnice sa vždy zmenia o vektor rýchlosti.
* Keďže bolo potrebná zaistiť, aby prvky samé od seba nezleteli z hracej plochy, vždy keď sa priblížia k jej okraju (okrem prípadu keď reagujú na náraz), otočia sa - môže sa preto stať, že sa niekedy zaseknú v pohybe hore-dole na tom istom úseku, no to je ich osud.
* Ak sú po náraze, kontroluje sa, či už neubehol čas určený na reakciu na náraz, ak áno, vektor sa pregeneruje
* Vektor sa pregeneruje aj po uplynutí intervalu, určujúceho frekvenciu zmien.  

Potomkami tejto triedy sú prvky __RedPupa__ a __BluePupa__, ktoré sa líšia len hodnotami atribútov. Za modré patrí viac bodov, keďže ich je ťažšie zhodiť z hracej plochy - majú väčšiu hmotnosť a nižší čas na zmenu smeru a reakciu na náraz.

##### Beecon

Od Pupas sa líši hlavne tým, že nie je konštantne v pohybe - hýbe sa len vtedy, keď doňho narazí motos, od toho sa odvýja aj jeho metóda `Pohyb()`: súradnice sa menia len ak je atribút `v_pohybe` nastavený na true, a kým sa nachádza v intervale reakcie na náraz. Za jeho zhodenie z hracej plochy je najviac bodov, lebo má najvyššiu hmotnosť a najkratšiu reakčnú dobu na náraz.  

#### Vektor

Som si skoro istá, že _C#_ má triedu na reprezentácio 2D vektorov, ale nechcelo sa mi googliť a čítať dokumentáciu na pochopenie jej fungovania, preto som si napísala vlastnú. Táto trieda má atribúty `x`, `y` a `norma`, ich účely sú myslím jednoznačné. Má prepísané operácie sčítania dvoch vektorov (+), násobenia vektoru skalárom (*) a vytvorenie vektoru s opačným smerom (-). Ďalej má metódu na výpočet normy, na vytvorenie normovaného vektoru k danému vektoru a na pregenerovanie nového náhodného vektoru (zmenia sa súradnice daného vektoru).  

## Priebeh práce

Priebeh práce je zachytený v súbore [ToDo.txt](https://gitlab.mff.cuni.cz/teaching/nprg031/2021-summer/student-zavodskg/-/blob/master/ToDo.txt), kde je popísaný nejaký ten postup a myšlienky.  
Najväčší problém mi robilo najprv ako vlastne začať a premyslieť si ako budú reprezentované prvky hry, keďže až tak nepoznám detaily C# alebo WinForms. Neskôr to bola fyzika, lebo, ako som zistila, si toho zo strednej školy veľa nepamätám. Musela som teda dlho googliť kým som si pripomenula, alebo skôr znovu pochopila, ako funguje hybnosť a zákon o zachovaní hybnosti. Tento problém mi prišiel celkom ironický, lebo si pamätám, že keď som si vyberala tému zápočtového programu, tak som si hovorila, že to bude lachké, veď tam je len ten jeden fyzikálny jav čo nám vysvetlovali na príklade s biliardom alebo s delom.